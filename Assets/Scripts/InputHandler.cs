using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField]
    private InputActionAsset inputActions;
    private InputAction moveAction;

    private void Start()
    {
        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");
    }

    public Vector2 MoveAxis()
    {
        return moveAction.ReadValue<Vector2>();
    }
}
