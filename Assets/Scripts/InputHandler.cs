using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputHandler : IInputHandler
{
    private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction fireAction;
    private InputAction pauseAction;

    public InputHandler(InputActionAsset inputActions)
    {
        this.inputActions = inputActions;

        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        fireAction = inputActions.FindAction("Attack");
        pauseAction = inputActions.FindAction("Pause");
        pauseAction.started += onPausePressed;
    }

    public bool isFiring()
    {
        return fireAction.IsPressed();
    }

    public void onPausePressed(InputAction.CallbackContext callbackContext) 
    {
        UIEvents.PausePressed();
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
