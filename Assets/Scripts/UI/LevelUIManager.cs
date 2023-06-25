using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIManager : SingletonMonobehavior<LevelUIManager>
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject mobileControlsPrefab;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        UpdateScore(GameManager.Instance.GetScore());
    }

    public void OnReturnToMenuClicked()
    {
        GameManager.Instance.BackToMainMenu();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " +score;
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void InitMobileInputUI()
    {
        Instantiate(mobileControlsPrefab, transform);
    }
}
