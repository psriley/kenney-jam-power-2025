using UnityEngine;
using UnityEngine.InputSystem;

public class CursorChangerSystem : MonoBehaviour
{
    public Texture2D PickaxeCursor;
    public Texture2D NormalCursor;
    public Texture2D InteractCursor;

    private void Update()
    {
        UpdateCursor();
    }

    void UpdateCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Texture2D cursorTexture = GetCursorTexture(null);

            if (hitObject.TryGetComponent(out ICursorHint cursor))
            {
                cursorTexture = GetCursorTexture(cursor);
            }
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            return;
        }

        // If nothing relevant is hit
        Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.Auto);
    }

    private Texture2D GetCursorTexture(ICursorHint cursor)
    {
        if (cursor == null) return NormalCursor;

        switch (cursor.GetCursorType()) { 
            case CursorType.Interact:
                return InteractCursor;
            case CursorType.Pickaxe:
                return PickaxeCursor;
            default: return NormalCursor;
        }
    }
}
