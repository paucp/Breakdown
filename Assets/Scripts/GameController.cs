using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private readonly object locker = new Object();
    private const int DefaultBuffTimeSeconds = 15;


    public GameObject TopDamageObject;
    public GameObject DoubleScoreObject;
    public GameObject BiggerPaddleObject;
    public GameObject BiggerBallObject;
    private Dictionary<BuffType, GameObject> BuffObjects;

    public Dictionary<BuffType, float> ActiveBuffs = new Dictionary<BuffType, float>();

    public AudioSource AudioSource;
    public AudioClip HitAudioClip;
    public AudioClip BrickBrokenAudioClip;
    public AudioClip BallLostAudioClip;
    public ParticleSystem BallParticleSystem;
    public Ball Ball;
    public Paddle Paddle;
    public Brick[] Bricks { get; private set; }

    public TextMeshProUGUI LivesText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimeText;

    public GameObject ScorePopTextPrefab;

    private float Timer = 0;

    public int Score = 0;
    public int Lives = 3;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void ResetGame()
    {
        Score = 0;
        Lives = 3;
        Timer = 0;
        LivesText.text = "Lives: 3";
        TimeText.text = "Time: 0";
        ScoreText.text = "Score: 0";
        Ball.Speed = 10f;
        ResetBallAndPaddle();
    }
    private void EnableBuff(BuffType buffType)
    {
        BuffObjects[buffType].SetActive(false);
        lock (locker) ActiveBuffs.Add(buffType, 0);
        if (buffType == BuffType.BiggerBall)
        {
            Ball.transform.localScale = new Vector3(4, 4, 0);
        }
        else if (buffType == BuffType.BiggerPaddle)
        {
            Paddle.transform.localScale = new Vector3(1.5f, 1, 0);
        }
        else if (buffType == BuffType.TopDamage)
        {
            Ball.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            Ball.GetComponentInChildren<TrailRenderer>().startColor = new Color(255, 0, 0);
            Ball.GetComponentInChildren<TrailRenderer>().endColor = new Color(255, 0, 0);
        }
        else if (buffType == BuffType.DoubleScore)
        {
            Paddle.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        }
    }
    private void DisableBuff(BuffType buffType)
    {
        lock (locker) ActiveBuffs.Remove(buffType);
        if (buffType == BuffType.BiggerBall)
        {
            Ball.transform.localScale = new Vector3(1, 1);
        }
        else if (buffType == BuffType.BiggerPaddle)
        {
            Paddle.transform.localScale = new Vector3(1, 1);
        }
        else if (buffType == BuffType.TopDamage)
        {
            Ball.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            Ball.GetComponentInChildren<TrailRenderer>().startColor = new Color(255, 255, 255);
            Ball.GetComponentInChildren<TrailRenderer>().endColor = new Color(255, 255, 255);
        }
        else if (buffType == BuffType.DoubleScore)
        {
            Paddle.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }
    private int GetUnBuffedBrick()
    {
        int brickNum = (int)Random.Range(0, Bricks.Length);
        while (Bricks[brickNum].BuffType != BuffType.None)
            brickNum = (int)Random.Range(0, Bricks.Length);
        return brickNum;
    }
    private void GiveBuff(int BrickNum, BuffType BuffType)
    {
        BuffObjects[BuffType].SetActive(true);
        Bricks[BrickNum].BuffType = BuffType;
        BuffObjects[BuffType].transform.position = Bricks[BrickNum].transform.position;
        BuffObjects[BuffType].transform.parent = Bricks[BrickNum].transform;
    }
    private void AssignBuffs()
    {
        foreach (Brick b in Bricks)
            b.BuffType = BuffType.None;
        GiveBuff(GetUnBuffedBrick(), BuffType.TopDamage);
        GiveBuff(GetUnBuffedBrick(), BuffType.DoubleScore);
        GiveBuff(GetUnBuffedBrick(), BuffType.BiggerBall);
        GiveBuff(GetUnBuffedBrick(), BuffType.BiggerPaddle);
    }
    private void Start()
    {
        Bricks = FindObjectsOfType<Brick>();
        BuffObjects = new Dictionary<BuffType, GameObject>() {
            { BuffType.TopDamage, TopDamageObject },
            { BuffType.DoubleScore, DoubleScoreObject },
            { BuffType.BiggerBall, BiggerBallObject },
            { BuffType.BiggerPaddle, BiggerPaddleObject },
        };
        AssignBuffs();
        ResetGame();
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        TimeText.text = "Time: " + (int)(Timer % 60);
        lock (locker)
        {
            foreach (var buff in ActiveBuffs)
            {
                ActiveBuffs[buff.Key] += Time.deltaTime;
                if (ActiveBuffs[buff.Key] % 60 >= DefaultBuffTimeSeconds)
                    DisableBuff(buff.Key);
            }
        }
    }

    private void ResetBallAndPaddle()
    {
        Ball.ResetBall();
        Paddle.ResetPaddle();
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetSceneByName("Game").isLoaded)
        {
            Bricks = FindObjectsOfType<Brick>();
            ResetGame();
        }
    }
    public void BallOutOfBounds()
    {
        Lives--;
        AudioSource.PlayOneShot(BallLostAudioClip);
        LivesText.text = "Lives: " + Lives;
        if (Lives > 0) ResetBallAndPaddle();
        else GameOver();
    }
    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    private bool MapCleared()
    {
        for (int i = 0; i < Bricks.Length; i++)
            if (Bricks[i].gameObject.activeInHierarchy)
                return false;
        return true;
    }
    public void NotifyBallHit()
    {
        AudioSource.PlayOneShot(HitAudioClip);
    }
    public void NotifyBallHitBrick()
    {
        BallParticleSystem.Play();
    }
    public void NotifyBrickBroken(Brick Brick)
    {
        AudioSource.PlayOneShot(BrickBrokenAudioClip);
        Ball.Speed += 0.2f;

        if (Brick.BuffType != BuffType.None)
            EnableBuff(Brick.BuffType);

        int GainedScore = Brick.BrickScorePoints;
        if (ActiveBuffs.ContainsKey(BuffType.DoubleScore))
            GainedScore *= 2;
        Score += GainedScore;
        GameObject prefab = Instantiate(ScorePopTextPrefab, Brick.transform.position, Quaternion.identity);
        prefab.GetComponentInChildren<TextMeshPro>().text = GainedScore.ToString();

        ScoreText.text = "Score: " + Score;
        if (MapCleared()) SceneManager.LoadScene("Win");
    }
}
