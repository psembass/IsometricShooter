using System;
using UnityEngine;

public static class UIEvents
{
    public static event Action OnRestart;

    public static void RestartButtonClicked() => OnRestart?.Invoke();
}
