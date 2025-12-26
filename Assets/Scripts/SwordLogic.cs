using System;
using Unity.VisualScripting;
using UnityEngine;

public class SwordLogic : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 250f;
    [SerializeField] private float orbitRadius = 3f;

    void Start()
    {
        // Position sword at initial orbit distance from parent
        transform.localPosition = new Vector3(orbitRadius, 1.5f, 0);
        transform.localRotation = Quaternion.Euler(90, 0, -90);
    }

    void Update()
    {
        // Rotate around the parent (empty GameObject)
        transform.RotateAround(transform.parent.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(50);
        }
    }
}
