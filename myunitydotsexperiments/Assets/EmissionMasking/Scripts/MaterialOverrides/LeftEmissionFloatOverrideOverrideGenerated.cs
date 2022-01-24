using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_LeftEmission", MaterialPropertyFormat.Float)]
    struct LeftEmissionFloatOverride : IComponentData
    {
        public float Value;
    }
}
