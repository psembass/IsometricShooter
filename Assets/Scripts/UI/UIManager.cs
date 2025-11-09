using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument gameOverMenu;
    [SerializeField]
    private UIDocument hud;

    private Label killCountLabel;
    private Label pauseLabel;
    private Label controlsHint;

    private void Awake()
    {
        VisualElement rootVisualElement = gameOverMenu.rootVisualElement;
        Button restartButton = gameOverMenu.rootVisualElement.Q<Button>("RestartButton");
        restartButton.clicked += RestartButtonClicked;
        gameOverMenu.rootVisualElement.style.display = DisplayStyle.None;

        killCountLabel = hud.rootVisualElement.Q<Label>("KillCountLabel");
        pauseLabel = hud.rootVisualElement.Q<Label>("PauseHint");
        pauseLabel.style.display = DisplayStyle.None;
        UIEvents.OnPause += ShowOrHidePause;
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void RestartButtonClicked()
    {
        gameOverMenu.rootVisualElement.style.display = DisplayStyle.None;
        UIEvents.RestartButtonClicked(); // send event outside
    }

    internal void UpdateKillCount(int enemyKilled)
    {
        killCountLabel.text = "Kill Count: "+enemyKilled;
    }

    public void HideControlsHint()
    {
        controlsHint.style.display = DisplayStyle.None;
    }

    private void ShowOrHidePause()
    {
        pauseLabel.style.display = pauseLabel.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void ShowControls(bool gamepad)
    {
        if (gamepad)
        {
            controlsHint = hud.rootVisualElement.Q<Label>("ControlsGamepadHint");
        }
        else
        {
            controlsHint = hud.rootVisualElement.Q<Label>("ControlsHint");
        }
        controlsHint.style.display = DisplayStyle.Flex;
    }
}
