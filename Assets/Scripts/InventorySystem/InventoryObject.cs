using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "Scriptable Objects/Inventory/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    public Inventory Container;

    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            InventorySlot slot = Container.Items[i];
            Item currentItem = slot.Item;
            bool isItem = currentItem == null ? false: currentItem.ID == item.ID;

            if (isItem)
            {
                slot.AddAmount(amount);
                return;
            }
        }
        Container.Items.Add(new InventorySlot(item.ID, item, amount));
    }

    public void Clear()
    {
        Container.Items.Clear();
    }
}

[System.Serializable]
public class Inventory
{
    // We do this so the List of items can be Serializable
    public List<InventorySlot> Items = new List<InventorySlot>();
}


[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item Item;
    public int Amount;

    public bool IsEmptySlot
    {
        get
        {
            return Item == null;
        }
    }

    public InventorySlot(int _id, Item _item, int _amount)
    {
        this.ID = _id;
        this.Item = _item;
        this.Amount = _amount;
    }

    public void AddAmount(int value)
    {
        this.Amount += value;
    }
}