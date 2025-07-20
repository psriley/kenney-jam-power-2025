using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PowerStorage", menuName = "Scriptable Objects/PowerStorage")]
public class PowerStorage : ScriptableObject
{
    public UnityEvent LowPower = new UnityEvent();

    public int Power = 1;

    public void Store(int amount)
    {
        Power += amount;
    }

    public void Drain(int amount)
    {
        Power -= amount;

        if (Power <= 0)
        {
            Power = 0;
            LowPower?.Invoke();
        }
    }

    public void ResetPower()
    {
        Power = 0;
    }
}
