using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private int _health = 100;
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
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
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

        Instantiate(deathParticleEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}