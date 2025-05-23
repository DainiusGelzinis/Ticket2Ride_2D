using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float resetPositionX = -20f; // when it reaches this point, it resets
    [SerializeField] private float startPositionX = 20f;  // the position to reset to

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}
