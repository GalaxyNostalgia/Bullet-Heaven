using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private int _health = 100;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Debug.Log($"Player has died! HP: {_health}");
        }
        else
        {
            Debug.Log($"HP: {_health}");
        }
    }
}
