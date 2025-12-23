using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject _player;
    private float _spawnInterval = 3f;
    private float _lastSpawnTime = 0f;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }
    
    
    void Update()
    {
        List<GameObject> enemies = new List<GameObject>();
        
    }
}
