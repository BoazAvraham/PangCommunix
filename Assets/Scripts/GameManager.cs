using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameManager : SingletonMonobehavior<GameManager>
{

    //Data - should be in another class
    private bool debugMod = false;
    [SerializeField] private GameObject wirePrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float minBallSize = 0.4f;
    [SerializeField] private int shotsLimit;
    [SerializeField] private float shotSpeed = 5f;
    [SerializeField] private float wireTimeOut = 2f;
    [SerializeField] private float scoringBonusInterval = 1f;
    [SerializeField] private int baseBallScore = 10;
    protected override void OnAwake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public float MinBallSize => minBallSize;
    public float ShotSpeed => shotSpeed;
    public float WireTimeOut => wireTimeOut;
    public GameObject GetWirePrefab() => wirePrefab;
    public GameObject GetBallPrefab() => ballPrefab;
    private bool IsLevelScene => SceneManager.GetActiveScene().buildIndex > 0;
    public int ShotsLimit => shotsLimit;

//logic

    private int score;
    public int GetScore() => score;
    
    private float lastScoreTime;
    private int lastScore;
    private void Start()
    {
        var val = PlayerPrefs.GetFloat("volume");
        SoundManager.Instance.SetVolume(val);
        lastScoreTime = -10;
    }

    public void GameOver()
    {
        LevelUIManager.Instance.OnGameOver();
        int highscore = PlayerPrefs.GetInt("Score");
        if (highscore < score)
            PlayerPrefs.SetInt("Score",score);
        score = 0;
    }

    public void SaveVolumeSettins(float val)
    {
        PlayerPrefs.SetFloat("volume",val);
        SoundManager.Instance.SetVolume(val);
    }
    
    
    
    //FOR TESTING
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && debugMod && IsLevelScene)
        {
            var ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ball.GetComponent<Ball>().Init(4,1.5f);
        }
    }
    
    private void OnValidate()
    {
        if (wirePrefab != null && wirePrefab.GetComponent<Transform>() == null)
        {
            Debug.LogError("The referenced prefab must have the Wire script attached.");
            wirePrefab = null; 
        }
    }
    
    public void AddScore()
    {
        
        if (Time.time - lastScoreTime > scoringBonusInterval)
            lastScore = baseBallScore;
        else
            lastScore *= 2;
        
        lastScoreTime = Time.time;
        score += lastScore;
        LevelUIManager.Instance.UpdateScore(score);
    }

    public int GetHighScore() => PlayerPrefs.GetInt("Score");
}
