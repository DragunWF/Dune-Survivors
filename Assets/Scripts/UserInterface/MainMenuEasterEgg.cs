using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEasterEgg : MonoBehaviour
{
    private const string EASTER_EGG_BUTTON = "EasterEggButton";
    private const string EASTER_EGG_PANEL = "EasterEggPanel";
    private const string EXIT_BUTTON = "ExitButton";

    private GameObject easterEggButton;
    private GameObject easterEggPanel;
    private GameObject exitButton;
    
    private void Awake()
    {
        easterEggButton = GameObject.Find(EASTER_EGG_BUTTON);
        easterEggPanel = GameObject.Find(EASTER_EGG_PANEL);
        exitButton = GameObject.Find(EXIT_BUTTON);
    }
    private void Start()
    {
        easterEggPanel.SetActive(false);
    }  

    public void OnEasterEggButtonClick()
    {
        easterEggPanel.SetActive(true);
    }

    public void OnExitButtonClick()
    {
        easterEggPanel.SetActive(false);
    }
}
