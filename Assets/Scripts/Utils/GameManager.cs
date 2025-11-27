using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int mainMenuIndex = 0;
    private const int gameSceneIndex = 1;
    private const int gameOverSceneIndex = 2;

    #region Audio

    [SerializeField] private Image soundOnIcon;
    [SerializeField] private Image soundOffIcon;
    private bool muted;

    #endregion

    #region Load Scene Methods

    public void LoadMainMenu() => LoadScene(mainMenuIndex);
    public void LoadGameScene() => LoadScene(gameSceneIndex);
    public void LoadGameOverScene() => LoadScene(gameOverSceneIndex);

    #endregion

    #region Getter Methods for Scene Indicies

    public int GetMainMenuIndex() => mainMenuIndex;
    public int GetGameSceneIndex() => gameSceneIndex;
    public int GetGameOverSceneIndex() => gameOverSceneIndex;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void Start()
    {
        muted = PlayerPrefs.GetInt("Muted", 0) == 1;
        ApplyMuteState();
    }

    public void ToggleMute()
    {
        muted = !muted;
        PlayerPrefs.SetInt("Muted", muted ? 1 : 0);
        ApplyMuteState();
    }

    private void ApplyMuteState()
    {
        AudioListener.volume = muted ? 0f : 1f;
        if (soundOnIcon == null)
        {
            Debug.Log("SoundOnIcon is not assigned in the inspector!");
            return;
        }
        if (soundOffIcon == null)
        {
            Debug.Log("SoundOffIcon is not assigned in the inspector!");
            return;
        }

        soundOnIcon.gameObject.SetActive(!muted);
        soundOffIcon.gameObject.SetActive(muted);
    }
}
