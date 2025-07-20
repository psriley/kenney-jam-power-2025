using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MineableItem : MonoBehaviour, IInteractable, ICursorHint
{
    [SerializeField] private InventoryObject Inventory;
    [SerializeField] private UIScriptableObject uiScriptableObject;
    [SerializeField] private ItemObject Item;
    [SerializeField] private int Yield = 1;
    [SerializeField] private int RemainingOre = 20;
    public UnityEvent<GridCell> OreDepleated;
    public UnityEvent MineSoundEvent;

    public CursorType GetCursorType() => CursorType.Pickaxe;

    private void Awake()
    {
        if (OreDepleated == null) { 
            OreDepleated = new UnityEvent<GridCell>();
        }
    }

    public void Interact()
    {
        RemainingOre -= 1;
        Inventory.AddItem(Item.CreateItem(), Yield);
        MineSoundEvent?.Invoke();

        uiScriptableObject.NumMetal += Yield;

        if (RemainingOre == 0)
        {
            OreDepleated?.Invoke(GetComponent<GridCell>());
        }
    }
}
