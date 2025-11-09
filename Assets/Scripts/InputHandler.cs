using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputHandler : IInputHandler
{
    private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction gpLookAction;
    private InputAction fireAction;
    private InputAction pauseAction;

    public InputHandler(InputActionAsset inputActions)
    {
        this.inputActions = inputActions;

        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        gpLookAction = inputActions.FindAction("GP_Look");
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

    public Vector2 LookPoint()
    {
        return lookAction.ReadValue<Vector2>();
    }

    public Vector2 LookVector()
    {
        return gpLookAction.ReadValue<Vector2>();
    }

    public Vector2 MoveAxis()
    {
        return moveAction.ReadValue<Vector2>();
    }

    public bool GamepadConnected()
    {
        return Gamepad.all.Count > 0;
    }
}
