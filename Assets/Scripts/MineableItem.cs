using UnityEngine;

public class MineableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryObject Inventory;
    [SerializeField] private ItemObject Item;
    [SerializeField] private int Yield;

    public void Interact()
    {
        Inventory.AddItem(Item.CreateItem(), Yield);
    }
}
