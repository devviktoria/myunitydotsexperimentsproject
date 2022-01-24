using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_BackEmission", MaterialPropertyFormat.Float)]
    struct BackEmissionFloatOverride : IComponentData
    {
        public float Value;
    }
}
