using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    #region

    private const string PAUSE_MENU = "PauseMenuPanel";

    #endregion

    private SceneFader sceneFader;
    private GameObject pauseMenu;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
        pauseMenu = GameObject.Find(PAUSE_MENU);
    }

    private void Start()
    {
        // Ensure the pause menu is hidden when the scene starts, after other scripts have potentially grabbed a reference in their Awake.
        pauseMenu.SetActive(false);
    }

    public void OnPauseButtonClick()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true);
    }

    public void OnResumeButtonClick()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false);
    }

    public void OnMainMenuButtonClick()
    {
        Time.timeScale = 1f; // Ensure time scale is reset before loading new scene
        sceneFader.FadeToMainMenu();
    }
}
