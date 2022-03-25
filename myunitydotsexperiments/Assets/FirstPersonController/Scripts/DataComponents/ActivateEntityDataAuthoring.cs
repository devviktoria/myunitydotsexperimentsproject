using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ActivateEntityData : IComponentData
{
    public Entity target;
}

[DisallowMultipleComponent]
public class ActivateEntityDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject target;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ActivateEntityData
        {
            target = conversionSystem.GetPrimaryEntity(target),
        });
    }
}
