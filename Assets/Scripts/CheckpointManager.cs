using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    int _highestCheckpointID = -1;
    Vector3 _checkpointPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Activate(int thisID, Vector3 thisPosition)
    {
        if (thisID > _highestCheckpointID)
        {
            _highestCheckpointID = thisID;
            _checkpointPosition = thisPosition;
            Debug.Log($"Checkpoint {thisID} at {thisPosition}");
        }
    }

    // ← And make sure this method is here:
    public Vector3 GetCheckpointPosition()
    {
        return _checkpointPosition;
    }

    public void ResetCheckpoints()
    {
        _highestCheckpointID  = -1;
        _checkpointPosition = Vector3.zero;
    }
}
