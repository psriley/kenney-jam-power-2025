using UnityEngine;

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