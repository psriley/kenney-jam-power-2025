using UnityEngine;

public class CrankSystem : MonoBehaviour, IPowerProducer, IInteractable, ICursorHint, IUpgradeable
{
    private int produce = 1;
    public int Produce => produce;
    public PowerStorage PowerStorage;
    public CostObject costObject;

    public CursorType GetCursorType() => CursorType.Interact;
    public CostObject CostObject() => costObject;

    public void Interact()
    {
        Debug.Log("Interacted with Crank System");
        PowerStorage.Store(Produce);
    }

    public void UpgradeLevel(int val)
    {
        produce += val;
    }
}
