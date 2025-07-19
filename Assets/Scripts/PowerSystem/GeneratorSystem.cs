using System;
using UnityEngine;

public class GeneratorSystem: MonoBehaviour, IPowerProducer, IUpgradeable
{
    public CostObject costObject;

    private int startProduce = 5;
    private int produce = 0;
    public int Produce => produce;

    public CostObject CostObject() => costObject;

    public void UpgradeLevel(int val)
    {
        if (produce == 0)
        {
            produce = startProduce;
            Debug.Log("Upgrade Generator to " + Produce);
            return;
        }

        produce += produce * val;
        Debug.Log("Upgrade Generator to " + Produce);
    }
}
