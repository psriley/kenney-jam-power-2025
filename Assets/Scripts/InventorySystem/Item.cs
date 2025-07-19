using UnityEngine;

[System.Serializable]
public class Item
{
    public int ID;
    public string Name;
    public Sprite UIDisplay;
    public ItemType Type;

    public Item(ItemObject itemObject)
    {
        ID = itemObject.ID;
        Name = itemObject.Name;
        UIDisplay = itemObject.UIDisplay;
        Type = itemObject.Type;
    }
}
