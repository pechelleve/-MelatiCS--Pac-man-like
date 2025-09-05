using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    private int _currentScore;
    private int _maxScore;

    public void AddScore(int value)
    {
        _currentScore += value;
        UpdateUI();
    }

    public void SetMaxScore(int value)
    {
        _maxScore = value;
        UpdateUI();
    }
    public void UpdateUI()
    {
        _scoreText.text = "Score: " + _currentScore + "/" + _maxScore;
    }

    void Start()
    {
        UpdateUI();
    }

}
