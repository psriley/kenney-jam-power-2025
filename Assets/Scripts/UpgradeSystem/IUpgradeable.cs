using UnityEngine;

public interface IUpgradeable
{
    public CostObject CostObject();
    public void UpgradeLevel(int val);
}
