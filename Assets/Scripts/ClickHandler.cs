using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{
    private UserInputActions inputActions;
    public UnityEvent<GridCell> buildEvent;

    private void Awake()
    {
        buildEvent = new UnityEvent<GridCell>();
        inputActions = new UserInputActions();
        inputActions.Power.Crank.performed += ctx => OnInteract();
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
}
