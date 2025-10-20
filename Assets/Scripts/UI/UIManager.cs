using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument gameOverMenu;

    private void Start()
    {
        VisualElement rootVisualElement = gameOverMenu.rootVisualElement;
        Button restartButton = gameOverMenu.rootVisualElement.Q<Button>("RestartButton");
        restartButton.clicked += RestartButtonClicked;
        gameOverMenu.rootVisualElement.style.display = DisplayStyle.None;
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
}
