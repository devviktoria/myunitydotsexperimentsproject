using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_TopEmission", MaterialPropertyFormat.Float)]
    struct TopEmissionFloatOverride : IComponentData
    {
        public float Value;
    }
}
