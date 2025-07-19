using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum ItemType
{
    Resources
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Inventory/Item")]
public class ItemObject : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite UIDisplay;
    public ItemType Type;

    public Item CreateItem()
    {
        return new Item(this);
    }
}