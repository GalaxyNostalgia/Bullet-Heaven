using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int MaxHealth { get; private set; } = 100;
    public int Health { get; private set; }
    [SerializeField] private GameObject deathParticleEffect;
    private Renderer _renderer;
    private Color _originalColor;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }
        Health = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
        else
        {
            if (_renderer != null)
            {
                StopAllCoroutines();
                StartCoroutine(FlashRed());
                
            }
        }
    }

    private IEnumerator FlashRed()
    {
        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        _renderer.material.color = _originalColor;
    }

    void Die()
    {
        GameObject playerRef = GameObject.Find("PlayerReference");
        if (playerRef != null)
        {
            playerRef.transform.position = transform.position;
        }

        GameObject particles = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        Destroy(particles, 0.5f);

        Destroy(gameObject);
    }
}