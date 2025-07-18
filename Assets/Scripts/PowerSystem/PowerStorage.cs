using UnityEngine;

[CreateAssetMenu(fileName = "PowerStorage", menuName = "Scriptable Objects/PowerStorage")]
public class PowerStorage: ScriptableObject
{
    public int Power = 0;

    public void Store(int amount)
    {
        Power += amount;
    }

    public void Drain(int amount)
    {
        Power -= amount;
    }

    public void ResetPower()
    {
        Power = 0;
    }
}
