using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputHandler : IInputHandler
{
    private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction fireAction;

    public InputHandler(InputActionAsset inputActions)
    {
        this.inputActions = inputActions;

        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        fireAction = inputActions.FindAction("Attack");
    }

    public bool isFiring()
    {
        return fireAction.IsPressed();
    }

    public Vector2 LookAxis()
    {
        return lookAction.ReadValue<Vector2>();
    }

    public Vector2 MoveAxis()
    {
        return moveAction.ReadValue<Vector2>();
    }
}
