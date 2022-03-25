using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class FollowDOTSCameraTarget : MonoBehaviour
{
    private EntityManager _entityManager;
    private EntityQuery _cameraTargetTagQuery;
    private Transform _transform = null;

    private void Awake()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _cameraTargetTagQuery = _entityManager.CreateEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
                {
                typeof(DotsCameraTargetData)
                }
        });
    }

    void LateUpdate()
    {
        if (_transform == null)
        {
            _transform = GetComponent<Transform>();
        }

        Entity cameraTargetEntity = _cameraTargetTagQuery.GetSingletonEntity();

        if (_entityManager.HasComponent<LocalToWorld>(cameraTargetEntity))
        {
            LocalToWorld cameraLocalToWorld = _entityManager.GetComponentData<LocalToWorld>(cameraTargetEntity);
            _transform.position = cameraLocalToWorld.Position;
            _transform.rotation = cameraLocalToWorld.Rotation;
        }
    }
}
