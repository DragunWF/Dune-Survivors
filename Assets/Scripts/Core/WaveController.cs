using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public sealed class WaveController : MonoBehaviour
{
    [SerializeField] private int currentWave = 1;

    private Wave[] waves;
    private GameSceneUI gameSceneUI;

    private TaskCompletionSource<bool> nextWaveTask;

    private void Awake()
    {
        gameSceneUI = FindObjectOfType<GameSceneUI>();

        // Wave difficulty settings
        waves = new Wave[]
        {
            new(30, 2.5f, 2, 1),  // Wave 1 
            new(45, 2.5f, 3, 1),  // Wave 2 
            new(70, 2.2f, 4, 2),  // Wave 3 
            new(90, 2.5f, 4, 2),  // Wave 4 
            new(120, 2.2f, 4, 3),  // Wave 5 
            new(150, 2.0f, 4, 3),  // Wave 6 
            new(180, 1.8f, 4, 4), // Wave 7 
            new(220, 1.8f, 4, 4), // Wave 8 
            new(250, 1.8f, 4, 5), // Wave 9 
            new(300, 1.6f, 4, 5)  // Wave 10 
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

            gameSceneUI.ShowWaveCompleteText();

            nextWaveTask = new TaskCompletionSource<bool>();
            yield return new WaitUntil(() => nextWaveTask.Task.IsCompleted);
        }
        // All waves completed. Handle game win condition or endless mode.
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
