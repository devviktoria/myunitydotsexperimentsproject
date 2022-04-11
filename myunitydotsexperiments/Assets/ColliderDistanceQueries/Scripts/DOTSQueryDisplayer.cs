using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class DOTSQueryDisplayer : MonoBehaviour
{
    public Entity hitDataEntity = Entity.Null;

    private EntityManager _entityManager;
    private bool Simulating;
    private DOTSQueryDisplayerData _dOTSQueryDisplayer;
    private CalculateDistanceQueryTesterSystem _myQueryTester;

    private void Awake()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Start()
    {
        Simulating = true;
    }

    void LateUpdate()
    {
        if (hitDataEntity == Entity.Null)
        {
            return;
        }

        _dOTSQueryDisplayer = _entityManager.GetComponentData<DOTSQueryDisplayerData>(hitDataEntity);
        _myQueryTester = World.DefaultGameObjectInjectionWorld.GetExistingSystem<CalculateDistanceQueryTesterSystem>();
    }

    void OnDrawGizmos()
    {
        if (Simulating)
        {
            ref PhysicsWorld world = ref World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
            // Draw distance hits
            // DistanceHit hit = _dOTSQueryDisplayer.distanceHit;
            // if (!_dOTSQueryDisplayer.distanceHitValid || hit.Entity == Entity.Null)
            // {
            //     return;
            // }
            int colliderRigidBodyIndex = world.GetRigidBodyIndex(hitDataEntity);
            NativeList<DistanceHit> distanceHits;
            if (_dOTSQueryDisplayer.QueryMode == QueryMode.AllDistanceHits)
            {
                distanceHits = _myQueryTester.distanceAllHit;
            }
            else if (_dOTSQueryDisplayer.QueryMode == QueryMode.ClosestHit)
            {
                distanceHits = _myQueryTester.distanceOneHit;
            }
            else if (_dOTSQueryDisplayer.QueryMode == QueryMode.OtherClosestHit)
            {
                distanceHits = _myQueryTester.distanceClosestHit;
            }
            else
            {
                distanceHits = _myQueryTester.distanceCustomHit;
            }

            if (distanceHits.IsCreated)
            {
                foreach (DistanceHit hit in distanceHits)
                {
                    Assert.IsTrue(hit.RigidBodyIndex >= 0 && hit.RigidBodyIndex < world.NumBodies);
                    Assert.IsTrue(math.abs(math.lengthsq(hit.SurfaceNormal) - 1.0f) < 0.01f);

                    float3 queryPoint = hit.Position + hit.SurfaceNormal * hit.Distance;

                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(hit.Position, 0.02f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(queryPoint, 0.02f);
                    Gizmos.color = colliderRigidBodyIndex == hit.RigidBodyIndex ? Color.red : Color.black;
                    Gizmos.DrawLine(hit.Position, queryPoint);

                    if (_dOTSQueryDisplayer.DrawSurfaceNormal)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawRay(hit.Position, hit.SurfaceNormal);
                    }

#if UNITY_EDITOR
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = colliderRigidBodyIndex == hit.RigidBodyIndex ? Color.red : Color.black;
                    Handles.Label(queryPoint, hit.Distance.ToString(), style);
#endif
                }
            }
        }
    }
}
