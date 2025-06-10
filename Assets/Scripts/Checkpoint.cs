using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointID = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log($"heckpoint {checkpointID} touched at {transform.position}");
        CheckpointManager.Instance.Activate(checkpointID, other.transform.position);
    }
}
