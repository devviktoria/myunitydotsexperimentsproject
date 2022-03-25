using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Float2InputData : IComponentData
{
    public float2 moveActionData;
    public float2 lookActionData;
}
