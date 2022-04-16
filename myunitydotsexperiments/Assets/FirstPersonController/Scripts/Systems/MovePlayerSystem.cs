using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

// public struct OtherNonTriggerClosestHitCollector<T> ....
// See in ColliderDistanceQueries directory

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(ExportPhysicsWorld))]
[UpdateBefore(typeof(EndFramePhysicsSystem))]
public partial class MovePlayerSystem : SystemBase
{
    BuildPhysicsWorld _BuildPhysicsWorldSystem;

    protected override void OnCreate()
    {
        _BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected unsafe override void OnUpdate()
    {
        Float2InputData float2InputData = GetSingleton<Float2InputData>();
        PhysicsWorld physicsWorld = _BuildPhysicsWorldSystem.PhysicsWorld;
        float deltaTime = Time.fixedDeltaTime;

        Dependency = Entities
            .WithoutBurst()
            .WithName("MovePlayer")
            .ForEach((Entity entity, ref PhysicsVelocity physicsVelocity,
                in LocalToWorld localToWorld,
                in CharacterControllerData characterControllerData,
                in PhysicsCollider collider,
                in PhysicsMass physicsMass,
                in Translation translation,
                in Rotation rotation) =>
            {
                float3 requestedMovementDirection = float3.zero;

                float rightHandsideMovement = float2InputData.moveActionData.x;
                float forwardMovement = float2InputData.moveActionData.y;
                if ((math.abs(rightHandsideMovement) > float.Epsilon) || (math.abs(forwardMovement) > float.Epsilon))
                {
                    float3 inputDirection = localToWorld.Forward * forwardMovement + localToWorld.Right * rightHandsideMovement;
                    requestedMovementDirection = math.normalize(inputDirection);
                }

                float3 linearVelocity = requestedMovementDirection * characterControllerData.movementSpeed;

                float3 distance = linearVelocity * deltaTime;
                float3 newPosition = translation.Value + distance;

                RigidTransform rigidTransform = new RigidTransform { pos = newPosition, rot = rotation.Value };

                ColliderDistanceInput colliderDistanceInput = new ColliderDistanceInput
                {
                    Collider = collider.ColliderPtr,
                    Transform = rigidTransform,
                    MaxDistance = 10f
                };

                int rigidBodyIndex = physicsWorld.GetRigidBodyIndex(entity);
                OtherNonTriggerClosestHitCollector<DistanceHit> otherNonTriggerClosestHitCollector =
                        new OtherNonTriggerClosestHitCollector<DistanceHit>(rigidBodyIndex, 10.0f);
                physicsWorld.CalculateDistance(colliderDistanceInput, ref otherNonTriggerClosestHitCollector);
                if (otherNonTriggerClosestHitCollector.NumHits > 0 &&
                    otherNonTriggerClosestHitCollector.closestHit.Distance > 0 &&
                    otherNonTriggerClosestHitCollector.closestHit.Distance < characterControllerData.skinWidth)
                {
                    linearVelocity = float3.zero;
                }

                float horizontalAngularRotation = float2InputData.lookActionData.x * characterControllerData.rotationSpeed;
                float3 horizontalAngularVelocity = math.up() * horizontalAngularRotation;

                physicsVelocity = new PhysicsVelocity { Linear = linearVelocity, Angular = horizontalAngularVelocity };

            }).Schedule(Dependency);

        Dependency.Complete();
    }
}
