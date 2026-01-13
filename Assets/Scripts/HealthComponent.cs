using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public static int ScoreStatic { get; private set; }
    public int MaxHealth { get; private set; } = 100;
    public int Health { get; private set; }
    
    [SerializeField] private GameObject deathParticleEffect;

    [Header("Flash")]
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private Color flashColor = Color.red;

    private Renderer[] _renderers;
    private MaterialPropertyBlock _mpb;

    
    private void Awake()
    {
        Health = MaxHealth;
        _renderers = GetComponentsInChildren<Renderer>(true);
        _mpb = new MaterialPropertyBlock();

    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        ApplyFlashColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        ClearFlashColor();
    }

    private void ApplyFlashColor(Color c)
    {
        foreach (var r in _renderers)
        {
            if (!r) continue;

            r.GetPropertyBlock(_mpb);

            _mpb.SetColor("_Color", c);
            _mpb.SetColor("_BaseColor", c);

            r.SetPropertyBlock(_mpb);
        }
    }

    private void ClearFlashColor()
    {
        // Clearing the property block restores the material’s original look.
        foreach (var r in _renderers)
        {
            if (!r) continue;
            r.SetPropertyBlock(null);
        }
    }

    private void Die()
    {
        GameObject playerRef = GameObject.Find("PlayerReference");
        if (playerRef != null)
            playerRef.transform.position = transform.position;

        GameObject particles = Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        Destroy(particles, 0.5f);

        Destroy(gameObject);

        if (CompareTag("Enemy"))
        {
            ScoreStatic++;
        }
    }
}