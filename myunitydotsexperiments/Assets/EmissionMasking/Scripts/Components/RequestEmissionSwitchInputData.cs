using Unity.Entities;

[GenerateAuthoringComponent]
public struct RequestEmissionSwitchInputData : IComponentData
{
    public UnityEngine.KeyCode topSwitchKeyCode;
    public UnityEngine.KeyCode backSwitchKeyCode;
    public UnityEngine.KeyCode leftSwitchKeyCode;
}
