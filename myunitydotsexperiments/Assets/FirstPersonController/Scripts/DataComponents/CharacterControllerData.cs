using Unity.Entities;

[GenerateAuthoringComponent]
public struct CharacterControllerData : IComponentData
{
    public float movementSpeed;
    public float rotationSpeed;
    public float skinWidth;
    public float characterMass;
}
