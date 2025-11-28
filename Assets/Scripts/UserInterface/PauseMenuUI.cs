using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public bool IsTransitioning { get; private set; }

    #region

    private const string PAUSE_MENU = "PauseMenuPanel";

    #endregion

    private SceneFader sceneFader;
    private SceneCleaner sceneCleaner;
    private GameObject pauseMenu;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
        sceneCleaner = FindObjectOfType<SceneCleaner>();
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
        Time.timeScale = 1.5f;
        IsTransitioning = true;
        sceneFader.SetFadeDuration(0.25f);
        sceneCleaner.DestroyAllProjectilesAndEffects();
        sceneFader.FadeToMainMenu();
    }
}
