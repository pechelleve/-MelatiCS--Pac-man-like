using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    [Header("UI")]
    [SerializeField] private TMP_Text _healthText;

    [Header("Respawn")]
    [SerializeField] private Transform _respawnPoint;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    void Start()
    {
        _currentHealth = _maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth > 0)
        {
            // Optional: Trigger a "hurt" animation
            // _animator.SetTrigger("Hurt");
            Respawn();
        }
        else
        {
            _currentHealth = 0;
            Die();
        }

        UpdateUI();
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;
        _currentHealth = Mathf.Min( _currentHealth, _maxHealth );
        UpdateUI();
    }

    private void Respawn()
    {
        // Teleport the player back to the respawn point
        transform.position = _respawnPoint.position;
        // Also reset velocity to stop them from flying off
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Optional: Trigger a death animation
        // _animator.SetTrigger("Die");
        SceneManager.LoadScene("LoseScreen");
    }

    void UpdateUI()
    {
        if (_healthText != null)
        {
            _healthText.text = "Health: " + _currentHealth;
        }
    }
}
