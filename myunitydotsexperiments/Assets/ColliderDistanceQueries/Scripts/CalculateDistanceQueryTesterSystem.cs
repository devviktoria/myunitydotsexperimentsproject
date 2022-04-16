using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine.Assertions;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

public struct OtherNonTriggerHitsCollector<T> : ICollector<T> where T : unmanaged, IQueryResult
{
    private int _selfRigidBodyIndex;
    public NativeList<T> AllHits;

    public bool EarlyOutOnFirstHit => false;

    public float MaxFraction { get; }

    public int NumHits => AllHits.Length;

    public OtherNonTriggerHitsCollector(int rigidBodyIndex, float maxFraction, ref NativeList<T> allHits)
    {
        _selfRigidBodyIndex = rigidBodyIndex;
        MaxFraction = maxFraction;
        AllHits = allHits;
    }

    public bool AddHit(T hit)
    {
        Assert.IsTrue(hit.Fraction < MaxFraction);

        if (hit.RigidBodyIndex == _selfRigidBodyIndex || hit.Material.CollisionResponse == CollisionResponsePolicy.RaiseTriggerEvents)
        {
            return false;
        }

        AllHits.Add(hit);
        return true;
    }
}

public struct OtherNonTriggerClosestHitCollector<T> : ICollector<T> where T : struct, IQueryResult
{
    private int _selfRigidBodyIndex;

    public T closestHit;

    public bool EarlyOutOnFirstHit => false;

    public float MaxFraction { get; private set; }

    public int NumHits { get; private set; }

    public OtherNonTriggerClosestHitCollector(int rigidBodyIndex, float maxFraction)
    {
        _selfRigidBodyIndex = rigidBodyIndex;
        MaxFraction = maxFraction;
        closestHit = default;
        NumHits = 0;
    }

    public bool AddHit(T hit)
    {
        Assert.IsTrue(hit.Fraction <= MaxFraction);

        if (hit.RigidBodyIndex == _selfRigidBodyIndex || hit.Material.CollisionResponse == CollisionResponsePolicy.RaiseTriggerEvents)
        {
            return false;
        }

        MaxFraction = hit.Fraction;
        closestHit = hit;
        NumHits = 1;
        return true;
    }
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(ExportPhysicsWorld))]
[UpdateBefore(typeof(EndFramePhysicsSystem))]
public partial class CalculateDistanceQueryTesterSystem : SystemBase
{
    public NativeList<DistanceHit> distanceOneHit;
    public NativeList<DistanceHit> distanceAllHit;
    public NativeList<DistanceHit> distanceCustomHit;
    public NativeList<DistanceHit> distanceClosestHit;
    private BuildPhysicsWorld _BuildPhysicsWorldSystem;

    protected override void OnCreate()
    {
        _BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        distanceOneHit = new NativeList<DistanceHit>(Allocator.Persistent);
        distanceAllHit = new NativeList<DistanceHit>(Allocator.Persistent);
        distanceCustomHit = new NativeList<DistanceHit>(Allocator.Persistent);
        distanceClosestHit = new NativeList<DistanceHit>(Allocator.Persistent);
    }

    protected unsafe override void OnUpdate()
    {
        PhysicsWorld world = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;

        Entities
            .WithoutBurst()
            .WithName("CalculateDistanceHit")
            .ForEach((Entity entity,
                ref DOTSQueryDisplayerData dOTSQueryDisplayerData,
                in PhysicsCollider collider,
                in Translation translation,
                in Rotation rotation) =>
            {
                float3 origin = translation.Value; //transform.position;
                ColliderDistanceInput colliderDistanceInput = new ColliderDistanceInput
                {
                    Collider = collider.ColliderPtr,
                    Transform = new RigidTransform(rotation.Value, origin),
                    MaxDistance = dOTSQueryDisplayerData.Distance
                };


                if (dOTSQueryDisplayerData.QueryMode == QueryMode.AllDistanceHits)
                {
                    distanceAllHit.Clear();
                    world.CalculateDistance(colliderDistanceInput, ref distanceAllHit);
                }
                else if (dOTSQueryDisplayerData.QueryMode == QueryMode.ClosestHit)
                {
                    distanceOneHit.Clear();
                    if (world.CalculateDistance(colliderDistanceInput, out DistanceHit distanceHit))
                    {
                        distanceOneHit.Add(distanceHit);
                    }
                }
                else if (dOTSQueryDisplayerData.QueryMode == QueryMode.OtherClosestHit)
                {
                    distanceClosestHit.Clear();
                    int rigidBodyIndex = world.GetRigidBodyIndex(entity);
                    OtherNonTriggerClosestHitCollector<DistanceHit> otherNonTriggerClosestHitCollector =
                        new OtherNonTriggerClosestHitCollector<DistanceHit>(rigidBodyIndex, dOTSQueryDisplayerData.Distance);
                    world.CalculateDistance(colliderDistanceInput, ref otherNonTriggerClosestHitCollector);
                    if (otherNonTriggerClosestHitCollector.NumHits > 0)
                    {
                        distanceClosestHit.Add(otherNonTriggerClosestHitCollector.closestHit);
                    }
                }
                else
                {
                    distanceCustomHit.Clear();
                    int rigidBodyIndex = world.GetRigidBodyIndex(entity);
                    OtherNonTriggerHitsCollector<DistanceHit> otherNonTriggerHitsCollector =
                         new OtherNonTriggerHitsCollector<DistanceHit>(rigidBodyIndex, dOTSQueryDisplayerData.Distance, ref distanceCustomHit);
                    world.CalculateDistance(colliderDistanceInput, ref otherNonTriggerHitsCollector);
                }

            }).Run();

        //Dependency.Complete();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        distanceOneHit.Dispose();
        distanceCustomHit.Dispose();
        distanceAllHit.Dispose();
        distanceClosestHit.Dispose();
    }
}
