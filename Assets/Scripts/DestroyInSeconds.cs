using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    private float SecondsToDestroy = 0.8f;
    void Start()
    {
        Destroy(gameObject, SecondsToDestroy);
    }
}
