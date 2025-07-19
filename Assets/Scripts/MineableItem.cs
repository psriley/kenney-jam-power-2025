using UnityEngine;

public class MineableItem : MonoBehaviour, IInteractable, ICursorHint
{
    [SerializeField] private InventoryObject Inventory;
    [SerializeField] private ItemObject Item;
    [SerializeField] private int Yield;

    public CursorType GetCursorType() => CursorType.Pickaxe;

    public void Interact()
    {
        Inventory.AddItem(Item.CreateItem(), Yield);
    }
}
