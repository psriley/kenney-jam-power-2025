using UnityEngine;

public class PowerStorage
{
    private int power = 0;
    private int Power {
        get
        {
            return power;
        }
    }

    public void Store(int amount)
    {
        power += amount;
    }

    public void Drain(int amount)
    {
        power -= amount;
    }


}
