using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsUI : MonoBehaviour
{
    private const string CONTROLS_MENU = "ControlsMenuPanel";
    private const string EXIT_BUTTON = "PauseExitButton";

    private GameObject exitButton;
    private GameObject controlsMenu;

    private void Awake()
    {
        controlsMenu = GameObject.Find(CONTROLS_MENU);
        exitButton = GameObject.Find(EXIT_BUTTON);
    }

    private void Start()
    {
        controlsMenu.SetActive(false);
    }

    public void OnControlsButtonClick()
    {
        controlsMenu.SetActive(true);
    }

    public void OnExitButtonClick()
    {
        controlsMenu.SetActive(false);
    }
}
