using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ActivationColorData : IComponentData
{
    public float4 deactivatedColor;
    public float4 activatedColor;
}

[DisallowMultipleComponent]
public class ActivationColorDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Color deactivatedColor;
    public Color activatedColor;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ActivationColorData
        {
            deactivatedColor = new float4(deactivatedColor.r, deactivatedColor.g, deactivatedColor.b, deactivatedColor.a),
            activatedColor = new float4(activatedColor.r, activatedColor.g, activatedColor.b, activatedColor.a)
        });
    }
}
