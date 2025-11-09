public sealed class Wave
{
    public int TimeLimit { get; private set; }
    public float SpawnInterval { get; private set; }
    public int EnemyTypeCount { get; private set; }
    public int EnemySpawnCountPerInterval { get; private set; } = 1;

    public Wave(int timeLimitInSeconds, float spawnInterval, int enemyTypeCount)
    {
        TimeLimit = timeLimitInSeconds;
        SpawnInterval = spawnInterval;
        EnemyTypeCount = enemyTypeCount;
    }

    public Wave(int timeLimitInSeconds, float spawnInterval, int enemyTypeCount, int enemySpawnCountPerInterval = 5)
    {
        TimeLimit = timeLimitInSeconds;
        SpawnInterval = spawnInterval;
        EnemyTypeCount = enemyTypeCount;
        EnemySpawnCountPerInterval = enemySpawnCountPerInterval;
    }
}
