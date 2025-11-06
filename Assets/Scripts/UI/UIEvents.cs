using System;
using UnityEngine;

public static class UIEvents
{
    public static event Action OnRestart;
    public static event Action OnPause;

    public static void RestartButtonClicked() => OnRestart?.Invoke();
    public static void PausePressed() => OnPause?.Invoke();
}
