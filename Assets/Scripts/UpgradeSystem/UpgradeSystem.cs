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

    public void UpgradeItem(IUpgradeable upgradeable)
    {
        if (HasFunds(upgradeable))
        {
            int cost = upgradeable.CostObject().Cost;
            ResourceCost resource = upgradeable.CostObject().ResourceCost;

            if (resource == ResourceCost.Energy)
            {
                powerStorage.Drain(cost);
            }
            if (resource == ResourceCost.Metal)
            {
                inventoryObject.GetSlotByID(0).Amount -= cost;
            }
            upgradeable.UpgradeLevel(1);
        }
        else
        {
            Debug.Log("Not enough resources to upgrade.");
        }
    }
}

