using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public Rigidbody2D RigidBody { get; private set; }
    public float Speed = 10f;
    private GameController GameController;

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        this.GameController = FindObjectOfType<GameController>();
    }
    private void Start()
    {
        ResetBall();
    }
    private void FixedUpdate()
    {
        RigidBody.velocity = RigidBody.velocity.normalized * Speed;
    }
    public void ResetBall()
    {
        RigidBody.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke(nameof(SetRandomTrajectory), 1f);
    }
    private void SetRandomTrajectory()
    {
        Vector2 Force = new Vector2();
        Force.x = Random.Range(-1f, 1f);
        Force.y = -1f;
        RigidBody.AddForce(Force.normalized * Speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("Brick"))
            GameController.NotifyBallHitBrick();
        GameController.NotifyBallHit();
    }
}
