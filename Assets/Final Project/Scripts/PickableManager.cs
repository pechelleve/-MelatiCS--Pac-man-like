using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickableManager : MonoBehaviour
{
    private List<Pickable> _pickableList = new List<Pickable>();
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerPowerUpHandler _playerPowerUpHandler;
    [SerializeField] private ScoreManager _scoreManager;

    [SerializeField] private AudioClip _powerActivatedSFX;
    [SerializeField] private AudioClip _coinSFX;

    private AudioSource _audioSource;


    private void Start()
    {
        InitPickableList();
    }

    private void InitPickableList()
    {
        Pickable[] pickableObjects = GameObject.FindObjectsOfType<Pickable>();
        for (int i = 0; i < pickableObjects.Length; i++)
        {
            _pickableList.Add(pickableObjects[i]);
            pickableObjects[i].OnPicked += OnPickablePicked;
        }
        _scoreManager.SetMaxScore(_pickableList.Count);
        Debug.Log("Pickable List: "+_pickableList.Count);
    }

    private void OnPickablePicked(Pickable pickable)
    {
        if (!_pickableList.Contains(pickable)) return;

        _pickableList.Remove(pickable);

        switch (pickable._pickableType)
        {
            case PickableType.PowerUp:
                _playerPowerUpHandler?.ActivatePowerUp();
                AudioSource.PlayClipAtPoint(_powerActivatedSFX, Camera.main.transform.position);
                break;

            case PickableType.Coin:
                _scoreManager?.AddScore(pickable.value);
                AudioSource.PlayClipAtPoint(_coinSFX, Camera.main.transform.position);
                break;

            case PickableType.Food:
                _playerHealth?.Heal(pickable.value);
                break;

        }
        Destroy(pickable.gameObject);

        if (_pickableList.Count <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
