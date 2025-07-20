using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraMoveScript : MonoBehaviour
{
    private UserInputActions inputActions;
    private Vector3 moveVector;

    [SerializeField] private float cameraMoveSpeed = 5f;
    [SerializeField] private float cameraFastMoveSpeed = 15f;
    private Vector3 defaultCamPosition = new Vector3(0,5.15f,0);
    private bool fastCam = false;


    private void Awake()
    {
        inputActions = new UserInputActions();
        inputActions.Player.Move.performed += ctx => MoveCamera(ctx);
        inputActions.Player.Move.canceled += ctx => MoveCamera(ctx);
        inputActions.Power.ResetCamToCrank.performed += ctx => ResetCamera();
        inputActions.Power.FastCamMove.started += ctx => fastCam = true;
        inputActions.Power.FastCamMove.canceled += ctx => fastCam = false;
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

    private void ResetCamera()
    {
        transform.position = defaultCamPosition;
    }

    private void Update()
    {
        var movementSpeed = cameraMoveSpeed;
        if (fastCam)
        {
            movementSpeed = cameraFastMoveSpeed;
        }
        
        transform.position += moveVector * movementSpeed * Time.deltaTime;
    }

}
