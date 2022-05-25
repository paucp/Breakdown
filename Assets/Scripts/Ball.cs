using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public Rigidbody2D RigidBody { get; private set; }
    public float Speed = 10f;

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetBall();
    }

    public void ResetBall()
    {
        RigidBody.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke(nameof(SetRandomTrajectory), 1f);
    }

    private void SetRandomTrajectory()
    {
        Vector2 force = new Vector2();
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        RigidBody.AddForce(force.normalized * Speed);
    }

    private void FixedUpdate()
    {
        RigidBody.velocity = RigidBody.velocity.normalized * Speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
