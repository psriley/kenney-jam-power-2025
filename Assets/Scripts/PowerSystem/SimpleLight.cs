using UnityEngine;

[CreateAssetMenu(fileName = "SimpleLight", menuName = "Scriptable Objects/Consumers/SimpleLight")]
public class SimpleLight : ScriptableObject, IPowerConsumer
{
    public int Consume => 1;
}
