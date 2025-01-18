using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private int _maxSpawnAmount = int.MaxValue;
    [SerializeField] private int _maxConcurrentEnemies = 4;
    private int _totalSpawned;
    private int _concurrentEnemies;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        while (_totalSpawned < _maxSpawnAmount && _concurrentEnemies < _maxConcurrentEnemies)
        {
            GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
            Vector2 spawnPos = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<Health>().OnDie.AddListener(() =>
            {
                _concurrentEnemies--;
                SpawnEnemies();
            });

            _totalSpawned++;
            _concurrentEnemies++;
        }
    }
}
