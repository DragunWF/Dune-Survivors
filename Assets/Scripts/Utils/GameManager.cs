using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    private const int mainMenuIndex = 0;
    private const int gameSceneIndex = 1;
    private const int gameOverSceneIndex = 2;

    #region Load Scene Methods

    public void LoadMainMenu() => LoadScene(mainMenuIndex);
    public void LoadGameScene() => LoadScene(gameSceneIndex);
    public void LoadGameOverScene() => LoadScene(gameOverSceneIndex);

    #endregion

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
