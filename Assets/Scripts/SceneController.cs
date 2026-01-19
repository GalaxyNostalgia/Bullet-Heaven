
using UnityEngine;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private GameObject _player;
    private float _spawnInterval = 5f;
    private float _lastSpawnTime;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }
    
    
    void Update()
    {
        OutOfBounds();
        if (Time.time >= _lastSpawnTime + _spawnInterval)
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnEnemy();
            }
            _lastSpawnTime = Time.time;
        }
        
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        int[] offsets = {-2, -1, 0, 1, 2};
        float[] offsetY = { 1, 1.5f, 2 };
        Vector3 offset = new Vector3(offsets[Random.Range(0, offsets.Length)], offsetY[Random.Range(0, offsetY.Length)], offsets[Random.Range(0, offsets.Length)]); 
        Instantiate(enemyPrefab, spawnPoints[index].position + offset, Quaternion.identity);
    }

    private void OutOfBounds()
    {
        if (!_player) return;
        
        if (_player.transform.position.y < -5)
        {
            _player.transform.position = new Vector3(0, 2f, 0);
        }
        
    }
}
