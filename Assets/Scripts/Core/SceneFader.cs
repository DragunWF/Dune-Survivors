using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Tooltip("The UI Image component that will be used for fading. It should cover the entire screen.")]
    public Image fadeImage;

    [Tooltip("The duration in seconds for the fade effect.")]
    public float fadeDuration = 1f;

    public static SceneFader Instance { get; private set; }

    private GameManager gameManager;

    private void Awake()
    {
        // Implement the Singleton pattern to ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Keep this object alive across scene loads
            DontDestroyOnLoad(gameObject);
        }

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

    /// <summary>
    /// Initiates a fade-to-black transition and then loads the specified scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void FadeToScene(string sceneName)
    {
        if (fadeImage == null)
        {
            Debug.LogError("SceneFader: Cannot fade, fadeImage is not assigned.");
            return;
        }
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
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

        // Load the new scene via the GameManager
        GameManager.Instance.LoadScene(sceneName);

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
