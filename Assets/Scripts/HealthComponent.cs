using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int MaxHealth { get; private set; } = 100;
    public int Health { get; private set; }

    [SerializeField] private GameObject deathParticleEffect;

    [Header("Flash")]
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private Color flashColor = Color.red;

    private Renderer[] _renderers;
    private MaterialPropertyBlock _mpb;

    // Cache original colors per renderer per material index.
    // Key: (renderer, materialIndex) -> original color
    private readonly Dictionary<(Renderer r, int i), Color> _originalColors = new();

    private void Awake()
    {
        Health = MaxHealth;

        _renderers = GetComponentsInChildren<Renderer>(true);
        _mpb = new MaterialPropertyBlock();

        CacheOriginalColors();
    }

    private void CacheOriginalColors()
    {
        _originalColors.Clear();

        foreach (var r in _renderers)
        {
            if (r == null) continue;

            var sharedMats = r.sharedMaterials;
            for (int i = 0; i < sharedMats.Length; i++)
            {
                var mat = sharedMats[i];
                if (mat == null) continue;

                // Most character shaders use _Color, some use _BaseColor (URP/HDRP).
                if (mat.HasProperty("_Color"))
                    _originalColors[(r, i)] = mat.GetColor("_Color");
                else if (mat.HasProperty("_BaseColor"))
                    _originalColors[(r, i)] = mat.GetColor("_BaseColor");
            }
        }
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
    }
}