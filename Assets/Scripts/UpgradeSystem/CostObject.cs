using UnityEngine;

public enum ResourceCost
{
    Energy,
    Metal
}

[CreateAssetMenu(fileName = "ObjectCost", menuName = "Scriptable Objects/ObjectCost")]
public class CostObject : ScriptableObject
{
    public int Cost;
    public ResourceCost ResourceCost;
}
