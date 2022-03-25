using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public partial class ChangeToActiveColorSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithName("ChangeToActiveColor")
            .ForEach((
                ref URPMaterialPropertyBaseColor uRPMaterialPropertyBaseColor,
                in ActiveData activeData,
                in ActivationColorData activationColorData) =>
            {
                if (activeData.active == 1)
                {
                    uRPMaterialPropertyBaseColor.Value = activationColorData.activatedColor;
                }
                else
                {
                    uRPMaterialPropertyBaseColor.Value = activationColorData.deactivatedColor;
                }
            }).Schedule();
    }
}
