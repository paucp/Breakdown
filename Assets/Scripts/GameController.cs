using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
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

    private float Timer = 0;
    private int TimerSeconds = 0;

    public int Score = 0;
    public int Lives = 3;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void ResetBallAndPaddle()
    {
        Ball.ResetBall();
        Paddle.ResetPaddle();
    }
    private void ResetGame()
    {
        Score = 0;
        Lives = 3;
        LivesText.text = "Lives: 3";
        TimeText.text = "Time: 0";
        ScoreText.text = "Score: 0";
        Ball.Speed = 10f;
        ResetBallAndPaddle();
    }
    private void Start()
    {
        Bricks = FindObjectsOfType<Brick>();
        ResetGame();
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        TimerSeconds = (int)(Timer % 60);
        TimeText.text = "Time: " + TimerSeconds;
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
    public void NotifyHitBrick(Brick Brick)
    {
        Score += Brick.BrickScorePoints;
        ScoreText.text = "Score: " + Score;

        if (MapCleared())
        {
            // WIN
        }
    }
    public void NotifyBallHit()
    {
        AudioSource.PlayOneShot(HitAudioClip);
    }
    public void NotifyBallHitBrick()
    {
        BallParticleSystem.Play();
    }
    public void NotifyBrickBroken()
    {
        AudioSource.PlayOneShot(BrickBrokenAudioClip);
        Ball.Speed += 0.2f;
    }
}
