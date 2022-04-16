using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;

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
