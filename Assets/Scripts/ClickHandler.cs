using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{
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

            if (hitObject.TryGetComponent(out IInteractable crank))
            {
                crank.Interact();
            }
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
