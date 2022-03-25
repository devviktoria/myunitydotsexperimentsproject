using Unity.Entities;

[GenerateAuthoringComponent]
public struct DotsCameraTargetData : IComponentData
{
    public float minVerticalAngle;
    public float maxVerticalAngle;
    public float currentVerticalAngle;
    public float verticalRotationSpeed;
}
