using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private GameObject[] _powerUpPrefabs;
    private GameManager _gameManager;
    private float _spawnRange = 9.0f;
    private float _spawnY = 0.5f;
    private int _enemyCount = 0;
    private int _waveNumber = 0;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }
    private void Update()
    {
        _enemyCount = FindObjectsOfType<Enemy>().Length;
        if(_enemyCount == 0)
        {
            _waveNumber++;
            _gameManager.Score = _waveNumber;
            SpawnEnemyWave(_waveNumber);
            SpawnPowerUp();
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyId = Random.Range(0, _enemyPrefabs.Length);
            Instantiate(_enemyPrefabs[enemyId], GenerateSpawnPos(), _enemyPrefabs[enemyId].transform.rotation);
        }
    }

    private void SpawnPowerUp()
    {
        int randomPowerup = Random.Range(0, _powerUpPrefabs.Length);
        Instantiate(_powerUpPrefabs[randomPowerup], GenerateSpawnPos(), _powerUpPrefabs[randomPowerup].transform.rotation);
    }

    private Vector3 GenerateSpawnPos()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange);
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        return new Vector3(spawnPosX, _spawnY, spawnPosZ);
    }
}
