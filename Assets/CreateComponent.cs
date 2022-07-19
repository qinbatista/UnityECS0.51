
using Unity.Entities;
[GenerateAuthoringComponent]
public struct CreateComponent : IComponentData
{
    public float value;
    public float amplitude;
    public float xOffset;
    public float yOffset;
}
