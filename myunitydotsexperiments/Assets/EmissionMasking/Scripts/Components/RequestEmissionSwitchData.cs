using Unity.Entities;

[GenerateAuthoringComponent]
public struct RequestEmissionSwitchData : IComponentData
{
    public bool topSwitchRequested;
    public bool backSwitchRequested;
    public bool leftSwitchRequested;
}
