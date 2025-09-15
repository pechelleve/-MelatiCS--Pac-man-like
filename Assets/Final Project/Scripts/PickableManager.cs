using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickableManager : MonoBehaviour
{
    private List<Pickable> _pickableList = new List<Pickable>();
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
        _pickableList.Remove(pickable);
        Destroy(pickable.gameObject);
        Debug.Log("Pickable List: " + _pickableList.Count);
        if (_pickableList.Count <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
        if (pickable._pickableType == PickableType.PowerUp)
        {
            AudioSource.PlayClipAtPoint(_powerActivatedSFX, Camera.main.transform.position);
            _playerPowerUpHandler?.ActivatePowerUp();
        }
        else if (pickable._pickableType == PickableType.Coin) 
        {
            AudioSource.PlayClipAtPoint(_coinSFX, Camera.main.transform.position);
        }
        if (_scoreManager != null)
        {
            _scoreManager.AddScore(1);
        }
    }
}
