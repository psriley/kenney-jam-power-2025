using UnityEngine;

public enum CursorType
{
    Normal,
    Interact,
    Pickaxe
}

public interface ICursorHint
{
    CursorType GetCursorType();
}
