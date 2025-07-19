using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{
    private UserInputActions inputActions;
    public UnityEvent<GridCell> buildEvent;
    public Texture2D PickaxeCursor;
    public Texture2D NormalCursor;
    public Texture2D InteractCursor;

    private void Awake()
    {
        buildEvent = new UnityEvent<GridCell>();
        inputActions = new UserInputActions();
        inputActions.UI.Click.performed += ctx => OnInteract();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void OnInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
            if (hitObject.TryGetComponent(out GridCell cell))
            {
                PlaceOnGridCell(cell);
            }
        }
    }


    void PlaceOnGridCell(GridCell cell)
    {
        if (!cell.IsOccupied)
        {
            buildEvent?.Invoke(cell);
        }
    }

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

            if (hitObject.TryGetComponent(out IInteractable _))
            {
                Cursor.SetCursor(InteractCursor, Vector2.zero, CursorMode.Auto);
                return;
            }
            else if (hitObject.TryGetComponent(out GridCell cell))
            {
                Cursor.SetCursor(PickaxeCursor, Vector2.zero, CursorMode.Auto);
                return;
            }
        }

        // If nothing relevant is hit
        Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.Auto);
    }
}
