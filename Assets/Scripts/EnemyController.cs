using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform _playerTransform; 
    private GameObject _player;
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    int _damage = 10; 
    int _speed = 1;
    float _attackCooldown = 1f;
    float _lastAttackTime = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
        _playerTransform = _player.transform;
        _renderer = _player.GetComponent<Renderer>();
    }
    
    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    public void MoveTowardsPlayer()
    {
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
        var healthComponent = player.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(_damage);
            _lastAttackTime = Time.time;
            StartCoroutine(FlashRed());
        }
        
        Vector3 knockbackDirection = (player.transform.position - transform.position + new Vector3((player.transform.position.x - transform.position.x)*10, 5, (player.transform.position.z - transform.position.z)*10)).normalized;
        player.GetComponent<PlayerController>().ApplyKnockback();
        player.GetComponent<Rigidbody>().AddForce(knockbackDirection * 10f, ForceMode.Impulse);

 
    }

    private IEnumerator FlashRed()
    {
        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        _renderer.material.color = Color.white;
    }
}
