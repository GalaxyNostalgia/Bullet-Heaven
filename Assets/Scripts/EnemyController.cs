
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform _playerTransform; 
    private GameObject _player;
    private Rigidbody _rigidbody;
    private int _damage = 10; 
    private int _speed = 1;
    private float _attackCooldown = 1f;
    private float _lastAttackTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
        _playerTransform = _player.transform;
    }
    
    void FixedUpdate()
    {
        if (_playerTransform)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (!_playerTransform) return;
    
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        direction.y = 0;
        Vector3 newPosition = _rigidbody.position + direction * (_speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(newPosition);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _rigidbody.MoveRotation(targetRotation);
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= _lastAttackTime + _attackCooldown)
        {
            DamagePlayer(collision.gameObject);
        }
    }

    private void DamagePlayer(GameObject player)
    {
        if (player == null) return;
    
        var playerController = player.GetComponent<PlayerController>();
        
        var healthComponent = player.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(_damage);
            _lastAttackTime = Time.time;
        }

        Vector3 knockbackDirection = (player.transform.position - transform.position + new Vector3((player.transform.position.x - transform.position.x)*10, 5, (player.transform.position.z - transform.position.z)*10)).normalized;
        var playerRigidbody = player.GetComponent<Rigidbody>();
    
        if (playerController != null) playerController.ApplyKnockback();
        if (playerRigidbody != null) playerRigidbody.AddForce(knockbackDirection * 10f, ForceMode.Impulse);
    }
    
}
