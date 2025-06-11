using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointID = 0;

    private Animator checkpointAnimator;
    void Start()
    {
        checkpointAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        checkpointAnimator.SetTrigger("Checkpoint");
        if (!other.CompareTag("Player")) return;
        Debug.Log($"heckpoint {checkpointID} touched at {transform.position}");
        CheckpointManager.Instance.Activate(checkpointID, other.transform.position);
    }
}
