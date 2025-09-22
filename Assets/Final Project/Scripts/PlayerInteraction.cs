using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require the components this script depends on
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerPowerUpHandler))]
public class PlayerInteraction : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private PlayerPowerUpHandler _powerUpHandler;

    [SerializeField] private float _interactRange;

    private void Awake()
    {
        // Get references to the other scripts on this same GameObject
        _playerHealth = GetComponent<PlayerHealth>();
        _powerUpHandler = GetComponent<PlayerPowerUpHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E)) 
        { 
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, _interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out Interactable interactible)) 
                {
                //    interactible.Interact(transform);
                }
        }
    }

    public Interactable GetInteractibleObject()
    {
        float _interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _interactRange);
        foreach (Collider collider in colliderArray)
            if (collider.TryGetComponent(out Interactable interactable))
            {
                return interactable;
            }
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Check the power-up handler to see if we are powered up
            if (_powerUpHandler.IsPoweredUp)
            {
                // Kill the enemy
                // A null check is good practice here
                if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(1);
                }
            }
            else
            {
                // If not powered up, take damage
                _playerHealth.TakeDamage(1);
            }
        }
    }

    // You can handle picking up powerups here too
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            _powerUpHandler.ActivatePowerUp();
            Destroy(other.gameObject); // Destroy the pickup
        }
    }
}
