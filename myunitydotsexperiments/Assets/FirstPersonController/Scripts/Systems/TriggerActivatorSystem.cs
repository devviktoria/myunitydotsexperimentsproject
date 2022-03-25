using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public partial class TriggerActivateRequestSystem : SystemBase
{
    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorldSystem;
    EntityQuery entitiesInScopeQuery;

    protected override void OnCreate()
    {
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();

        entitiesInScopeQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(ActivateEntityData) }
        });
    }

    //[BurstCompile]
    struct TriggerActivateRequestJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> playerTagGroup;
        public ComponentDataFromEntity<ActivateEntityData> activateEntityDataGroup;
        public ComponentDataFromEntity<ActiveData> activeDataComponents;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (entityA.Index <= 0 || entityB.Index <= 0 || entityA.Version < 0 || entityB.Version < 0)
            {

                return;
            }

            bool isBodyAPlayer = playerTagGroup.HasComponent(entityA);
            bool isBodyBPlayer = playerTagGroup.HasComponent(entityB);

            bool isBodyAOther = activateEntityDataGroup.HasComponent(entityA);
            bool isBodyBOther = activateEntityDataGroup.HasComponent(entityB);

            if (!(isBodyAOther && isBodyBPlayer || isBodyBOther && isBodyAPlayer))
            {
                return;
            }

            Entity activateEntity = isBodyAOther ? entityA : entityB;
            ActivateEntityData activateEntityData = activateEntityDataGroup[activateEntity];

            ActiveData activeData = activeDataComponents[activateEntityData.target];
            activeData.active = 1;
            activeDataComponents[activateEntityData.target] = activeData;
        }
    }

    protected override void OnUpdate()
    {
        if (entitiesInScopeQuery.CalculateEntityCount() == 0)
        {
            return;
        }

        Dependency = Entities
            .WithName("ClearActivateData")
            .ForEach((ref ActiveData activateData) =>
            {
                activateData.active = 0;
            }).Schedule(Dependency);

        Dependency.Complete();

        Dependency = new TriggerActivateRequestJob
        {
            playerTagGroup = GetComponentDataFromEntity<PlayerTag>(true),
            activateEntityDataGroup = GetComponentDataFromEntity<ActivateEntityData>(false),
            activeDataComponents = GetComponentDataFromEntity<ActiveData>(false)
        }.Schedule(stepPhysicsWorldSystem.Simulation, Dependency);
        Dependency.Complete();
    }
}