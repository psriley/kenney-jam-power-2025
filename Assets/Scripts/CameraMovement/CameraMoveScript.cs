using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraMoveScript : MonoBehaviour
{
    private UserInputActions inputActions;
    private Vector3 moveVector;

    [SerializeField] private float cameraMoveSpeed = 5f;


    private void Awake()
    {
        inputActions = new UserInputActions();
        inputActions.Player.Move.performed += ctx => MoveCamera(ctx);
        inputActions.Player.Move.canceled += ctx => MoveCamera(ctx);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void MoveCamera(CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        moveVector = new Vector3(input.x, 0, input.y); // X/Z plane movement
    }

    private void Update()
    {
        transform.position += moveVector * cameraMoveSpeed * Time.deltaTime;
    }

}
