using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OutOfBoundsZone : MonoBehaviour
{
    GameObject gc;
    public void Start()
    {
        gc = GameObject.Find("GameController");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        gc.GetComponent<GameController>().BallOutOfBounds();
    }
}
