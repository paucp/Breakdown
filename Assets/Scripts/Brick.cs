using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Brick : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Sprite[] BrickStates = new Sprite[0];
    public int Health { get; private set; }
    public int ScorePoints = 100;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetBrick();
    }

    public void ResetBrick()
    {
        gameObject.SetActive(true);
        Health = BrickStates.Length; 
    }

    private void BrickHit()
    {
        Health--;
        if (Health <= 0)  gameObject.SetActive(false); 
        else SpriteRenderer.sprite = BrickStates[Health - 1];
        FindObjectOfType<GameController>().Hit(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //
        if (collision.gameObject.name == "Ball") {
            BrickHit();
        }
    }

}
