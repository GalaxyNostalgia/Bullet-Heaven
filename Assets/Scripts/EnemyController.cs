using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform _player; 
    private Rigidbody _rigidbody;
    int _damage = 10; 
    int _speed = 1;
    float _attackCooldown = 1f;
    float _lastAttackTime = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }
    
    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    public void MoveTowardsPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        direction.y = 0;
        Vector3 newPosition = _rigidbody.position + direction * (_speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(newPosition);
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, 100f * Time.fixedDeltaTime));
        
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
        var healthComponent = player.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(_damage);
            _lastAttackTime = Time.time;
        }
        
        Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
        knockbackDirection.z = 0; 
        player.GetComponent<Rigidbody>().AddForce(knockbackDirection * 5, ForceMode.Impulse);
 
    }
}
