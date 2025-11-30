using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public sealed class WaveController : MonoBehaviour
{
    [SerializeField] private int currentWave = 1;

    private Wave[] waves;
    private GameSceneUI gameSceneUI;
    private SceneFader sceneFader;
    private GameStats gameStats;
    private Player player;

    private TaskCompletionSource<bool> nextWaveTask;

    public int CurrentWaveNumber => currentWave;

    private void Awake()
    {
        gameSceneUI = FindObjectOfType<GameSceneUI>();
        sceneFader = FindObjectOfType<SceneFader>();
        gameStats = FindObjectOfType<GameStats>();
        player = FindObjectOfType<Player>();

        // Wave difficulty settings
        waves = new Wave[]
        {
            new(30, 2.5f, 2, 1),  // Wave 1 
            new(45, 2.5f, 3, 2),  // Wave 2 
            new(55, 2.4f, 3, 2),  // Wave 3 
            new(55, 2.3f, 3, 3),  // Wave 4 
            new(60, 2.3f, 4, 3),  // Wave 5 
            new(60, 2.1f, 4, 3),  // Wave 6 
            new(65, 2.0f, 4, 4), // Wave 7 
            new(70, 1.5f, 4, 4), // Wave 8 
            new(75, 1.2f, 4, 5), // Wave 9 
            new(75, 0.8f, 4, 6)  // Wave 10 
        };
    }

    private void Start()
    {
        StartCoroutine(WaveProgressionCoroutine());
    }

    private IEnumerator WaveProgressionCoroutine()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            currentWave = i + 1; // Update currentWave for the UI and GetCurrentWave()
            Wave wave = GetCurrentWave(); // Get the current wave data

            gameSceneUI.UpdateWaveUI(currentWave, waves.Length);

            int secondsPassed = 0;
            while (secondsPassed < wave.TimeLimit)
            {
                gameSceneUI.UpdateWaveTimerUI(wave.TimeLimit - secondsPassed);
                yield return new WaitForSeconds(1f);
                secondsPassed++;
            }

            if (!player.IsDead)
            {
                gameSceneUI.ShowWaveCompleteText();
            }

            nextWaveTask = new TaskCompletionSource<bool>();
            yield return new WaitUntil(() => nextWaveTask.Task.IsCompleted);
        }

        const float gameCompletionDelay = 3f;
        gameSceneUI.ShowAllWaveCompletedText();
        gameStats.OnGameEnd(true);
        yield return new WaitForSeconds(gameCompletionDelay);
        sceneFader.FadeToGameOverScene();

        // All waves completed. Handle game win condition.
        Debug.Log("All waves completed!");
    }

    public Wave GetCurrentWave()
    {
        if (currentWave - 1 < waves.Length)
        {
            return waves[currentWave - 1];
        }
        else if (currentWave <= 0)
        {
            return waves[0];
        }
        else
        {
            return waves[^1]; // Return last wave if exceeded
        }
    }

    public void StartNextWave()
    {
        nextWaveTask?.TrySetResult(true);
    }
}
