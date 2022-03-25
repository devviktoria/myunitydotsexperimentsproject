using Unity.Entities;

[GenerateAuthoringComponent]
public struct ActiveData : IComponentData
{
    public int active;
}
