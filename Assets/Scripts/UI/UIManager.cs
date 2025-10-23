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

    private void Awake()
    {
        VisualElement rootVisualElement = gameOverMenu.rootVisualElement;
        Button restartButton = gameOverMenu.rootVisualElement.Q<Button>("RestartButton");
        restartButton.clicked += RestartButtonClicked;
        gameOverMenu.rootVisualElement.style.display = DisplayStyle.None;

        killCountLabel = hud.rootVisualElement.Q<Label>("KillCountLabel");
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
}
