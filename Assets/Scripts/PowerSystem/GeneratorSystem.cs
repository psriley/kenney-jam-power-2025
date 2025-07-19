using System;
using UnityEngine;

public class GeneratorSystem: MonoBehaviour, IPowerProducer, IUpgradeable
{
    public CostObject costObject;

    private int productionPerGen = 5;
    public int numGenerators = 0;
    public int Produce => numGenerators * productionPerGen;

    public CostObject CostObject() => costObject;

    public void UpgradeLevel(int val)
    {
        numGenerators += val;
        Debug.Log("Upgrade Generator to " + Produce);
    }
}
