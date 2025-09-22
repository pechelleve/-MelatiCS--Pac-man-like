using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    [Header("UI")]
    [SerializeField] private TMP_Text _healthText;

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
        }
        else
        {
            _currentHealth = 0;
            Die();
        }

        UpdateUI();
    }

    private void Die()
    {
        Debug.Log("Enemy has died.");
        // Optional: Trigger a death animation
        // _animator.SetTrigger("Die");
        Destroy(gameObject);
    }

    void UpdateUI()
    {
        if (_healthText != null)
        {
            _healthText.text = "Health: " + _currentHealth;
        }
    }
}
