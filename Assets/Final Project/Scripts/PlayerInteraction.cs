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

    private void Awake()
    {
        // Get references to the other scripts on this same GameObject
        _playerHealth = GetComponent<PlayerHealth>();
        _powerUpHandler = GetComponent<PlayerPowerUpHandler>();
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
                if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.Dead();
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
