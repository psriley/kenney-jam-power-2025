using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InteractionType { INTERACT, PLACE_TORCH }

public class ClickHandler : MonoBehaviour
{
    public InteractionType interactType = InteractionType.PLACE_TORCH;
    private UserInputActions inputActions;

    private void Awake()
    {
        inputActions = new UserInputActions();
        inputActions.Enable();

        inputActions.UI.Click.performed += ctx => OnInteract();
    }

    void OnInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Debug.Log(hitObject);

            if (hitObject.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
            if (hitObject.TryGetComponent(out GridCell cell))
            {
                // check what UI square is selected
                PlaceTorch(cell);
            }

            // if (interactType == InteractionType.INTERACT)
            // {
            //     if (hitObject.TryGetComponent(out IInteractable interactable))
            //     {
            //         interactable.Interact();
            //     }
            // }

            // else if (interactType == InteractionType.PLACE_TORCH)
            // {
            //     if (hitObject.TryGetComponent(out GridCell cell))
            //     {
            //         PlaceTorch(cell);
            //     }
            // }
        }
    }

    void PlaceTorch(GridCell cell)
    {
        if (!cell.isOccupied)
        {
            Debug.Log("Placing torch...");
        }
    }

    void Update()
    {

    }

    // private void OnDisable()
    // {
    //     inputActions.Player.Click.performed -= OnClick;
    //     inputActions.Player.Disable();
    // }

    // private void OnClick()
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    //     if (Physics.Raycast(ray, out RaycastHit hit))
    //     {
    //         Crank crank = hit.collider.GetComponent<Crank>();
    //         if (crank != null)
    //         {
    //             crank.Activate();
    //         }
    //     }
    // }
}
