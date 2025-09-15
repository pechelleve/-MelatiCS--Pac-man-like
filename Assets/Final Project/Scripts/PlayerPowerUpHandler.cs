using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayerPowerUpHandler : MonoBehaviour
{
    [Header("Power-up Settings")]
    [SerializeField] private float _powerupDuration = 10f;

    [Header("SFX")]
    [SerializeField] private AudioClip _powerDeactivatedSFX;

    // Use a public property with a private setter for better encapsulation
    public bool IsPoweredUp { get; private set; }

    // Actions for other scripts to subscribe to
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;

    private Coroutine _powerupCoroutine;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ActivatePowerUp()
    {
        // If a power-up is already active, stop the old one to reset the timer
        if (_powerupCoroutine != null)
        {
            StopCoroutine(_powerupCoroutine);
        }
        _powerupCoroutine = StartCoroutine(PowerUpSequence());
    }

    private IEnumerator PowerUpSequence()
    {
        IsPoweredUp = true;
        OnPowerUpStart?.Invoke(); // Fire event
        Debug.Log("Power-up Started");

        yield return new WaitForSeconds(_powerupDuration);

        IsPoweredUp = false;
        OnPowerUpStop?.Invoke(); // Fire event
        _audioSource.PlayOneShot(_powerDeactivatedSFX);
        Debug.Log("Power-up Ended");
    }
}
