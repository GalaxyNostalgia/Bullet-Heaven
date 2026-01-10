using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    private Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (_player)
        {
            transform.position = _player.position;
        }
    }
}
