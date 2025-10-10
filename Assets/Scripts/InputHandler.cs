using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputHandler : IInputHandler
{
    private InputActionAsset inputActions;
    private InputAction moveAction;

    public InputHandler(InputActionAsset inputActions)
    {
        this.inputActions = inputActions;

        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");
    }

    public Vector2 MoveAxis()
    {
        return moveAction.ReadValue<Vector2>();
    }
}
