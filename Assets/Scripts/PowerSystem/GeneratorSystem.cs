using UnityEngine;

public class GeneratorSystem: MonoBehaviour, IPowerProducer, IUpgradeable
{
    public CostObject costObject;

    public int startProduce = 5;
    public int produce = 0;
    public int Produce => produce;

    public GeneratorSystem(CostObject costObject)
    {
        this.costObject = costObject;
    }

    public CostObject CostObject() => costObject;

    public void UpgradeLevel(int val)
    {
        if (val == 0)
        {
            produce = startProduce;
            return;
        }

        produce += produce * val;
    }
}
