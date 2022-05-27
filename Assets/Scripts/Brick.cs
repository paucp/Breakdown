using UnityEngine;


public enum BuffType
{
    None,
    TopDamage,
    DoubleScore,
    BiggerPaddle,
    BiggerBall
}
[RequireComponent(typeof(BoxCollider2D))]
public class Brick : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Sprite[] BrickStates = new Sprite[0];
    private GameController GameController;
    public int BrickHealth { get; private set; }
    public int BrickScorePoints = 100;

    public BuffType BuffType = BuffType.None;
    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        this.GameController = FindObjectOfType<GameController>();
    }
    private void Start()
    {
        ResetBrick();
    }
    public void ResetBrick()
    {
        gameObject.SetActive(true);
        BrickHealth = BrickStates.Length; 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball") {
            if (GameController.ActiveBuffs.ContainsKey(BuffType.TopDamage)) BrickHealth = 0;
            else BrickHealth--;
            if (BrickHealth <= 0)
            {
                gameObject.SetActive(false);
                GameController.NotifyBrickBroken(this);
            }
            else SpriteRenderer.sprite = BrickStates[BrickHealth - 1];
        }
    }
}