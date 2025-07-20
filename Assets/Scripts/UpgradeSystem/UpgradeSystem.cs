using Mono.Cecil;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public InventoryObject inventoryObject;
    public PowerStorage powerStorage;

    public bool HasFunds(IUpgradeable upgradeable)
    {
        if (upgradeable.CostObject().ResourceCost == ResourceCost.Energy)
        {
            return powerStorage.Power >= upgradeable.CostObject().Cost;
        }
        if (upgradeable.CostObject().ResourceCost == ResourceCost.Metal)
        {
            return inventoryObject.GetItemAmount(0) >= upgradeable.CostObject().Cost;
        }
        return false;
    }

    public void UpgradeItem(IUpgradeable upgradeable, bool drainStorage)
    {
        CostObject costObject = upgradeable.CostObject();
        int cost = costObject.Cost;
        ResourceCost resource = costObject.ResourceCost;

        if (resource == ResourceCost.Energy && drainStorage)
        {
            powerStorage.Drain(cost);
        }
        if (resource == ResourceCost.Metal && drainStorage)
        {
            inventoryObject.GetSlotByID(0).Amount -= cost;
        }

        upgradeable.UpgradeLevel(1);
    }

    public void IncreaseCost(CostObject costObject)
    {
        if (costObject.ResourceCost == ResourceCost.Metal)
        {
            return;
        }

        costObject.Cost *= 4;
    }
}

