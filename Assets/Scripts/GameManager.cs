using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameManager : SingletonMonobehavior<GameManager>
{

    //Data - should be in another class
    private bool debugMod = false;
    private bool isMobile;
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
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        DontDestroyOnLoad(gameObject);
    }
    
    public float MinBallSize => minBallSize;
    public float ShotSpeed => shotSpeed;
    public float WireTimeOut => wireTimeOut;
    public GameObject GetWirePrefab() => wirePrefab;
    public GameObject GetBallPrefab() => ballPrefab;
    private bool IsLevelScene => SceneManager.GetActiveScene().buildIndex > 0;
    public int ShotsLimit => shotsLimit;
    public event Action OnLevelCompleted;
    public bool IsMobile => isMobile;

    //logic
    
    private int score;
    public int GetScore() => score;
    
    private float lastScoreTime;
    private int lastScore;
    private int ballsCount;
    private void Start()
    {
        var val = PlayerPrefs.GetFloat("volume");
        SoundManager.Instance.SetVolume(val);
    }

    public void GameOver()
    {
        LevelUIManager.Instance.OnGameOver();
        int highscore = PlayerPrefs.GetInt("Score");
        if (highscore < score)
            PlayerPrefs.SetInt("Score",score);
    }

    public void SaveVolumeSettins(float val)
    {
        PlayerPrefs.SetFloat("volume",val);
        SoundManager.Instance.SetVolume(val);
    }
    
    
    
    //FOR TESTING
    private void Update()
    {
        if(debugMod && IsLevelScene)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
                ball.GetComponent<Ball>().Init(4,1.5f , 0.5f, 2, Color.blue);    
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                AdvanceLevel();
            }
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

    public void StartGame()
    {
        score = 0;
        lastScoreTime = -10;
        AdvanceLevel();
    } 
    public int GetHighScore() => PlayerPrefs.GetInt("Score");
    public void RegisterBall() => ballsCount++;
    public void UnregisterBall()
    {
        IEnumerator DecreaseAndCheck()
        {
            ballsCount--;
            yield return new WaitForSeconds(1);
            Debug.Log("balls count: " + ballsCount);
            if (ballsCount == 0)
            {
                if (GameObject.FindWithTag("Player") == null)
                    yield break;
                OnLevelCompleted?.Invoke();
                yield return new WaitForSeconds(3);
                AdvanceLevel();
            }
        }

        StartCoroutine(DecreaseAndCheck());
    }

    private void AdvanceLevel()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        ballsCount = 0;
        if (SceneManager.sceneCountInBuildSettings <= nextSceneIndex)
        {
            Debug.Log("No next level");
            return;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
