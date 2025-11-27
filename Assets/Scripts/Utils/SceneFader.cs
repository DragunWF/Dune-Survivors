using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Tooltip("The UI Image component that will be used for fading. It should cover the entire screen.")]
    [SerializeField] private Image fadeImage;

    [Tooltip("The duration in seconds for the fade effect.")]
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private float delayBeforeSceneLoad = 1.5f;

    private GameManager gameManager;
    private SceneCleaner sceneCleaner;

    private void Awake()
    {
        // Ensure the fade image is initially transparent and inactive
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            fadeImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("SceneFader: Fade Image is not assigned! Please assign a UI Image to the 'fadeImage' field in the Inspector.");
        }

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        sceneCleaner = FindObjectOfType<SceneCleaner>();
    }

    #region Fade to Scene Loader Methods

    public void FadeToGameOverScene()
    {
        StartCoroutine(FadeOutAndLoad(gameManager.GetGameOverSceneIndex()));
    }

    public void FadeToMainMenu()
    {
        StartCoroutine(FadeOutAndLoad(gameManager.GetMainMenuIndex()));
    }

    public void FadeToGameScene()
    {
        StartCoroutine(FadeOutAndLoad(gameManager.GetGameSceneIndex()));
    }

    #endregion

    private IEnumerator FadeOutAndLoad(int sceneIndex)
    {
        // Activate the fade image and set it to fully transparent initially
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration); // Gradually increase alpha
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully black before loading the scene
        fadeImage.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(delayBeforeSceneLoad);
        if (sceneCleaner != null)
        {
            sceneCleaner.DestroyAllProjectilesAndEffects();
        }
        else
        {
            Debug.Log("SceneCleaner class is not present in the current scene!");
        }
        GameManager.Instance.LoadScene(sceneIndex);

        // After loading the new scene, immediately start fading in from black
        // This ensures the new scene appears with a fade-in effect.
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Initiates a fade-in-from-black transition. This is typically called automatically
    /// after a scene is loaded via FadeToScene, but can be called manually if needed.
    /// </summary>
    public void FadeInFromBlack()
    {
        if (fadeImage == null)
        {
            Debug.LogError("SceneFader: Cannot fade, fadeImage is not assigned.");
            return;
        }
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // Ensure the image is active and fully opaque initially
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 1);

        float timer = fadeDuration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration); // Gradually decrease alpha
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully transparent and then deactivate the image
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);
    }
}
