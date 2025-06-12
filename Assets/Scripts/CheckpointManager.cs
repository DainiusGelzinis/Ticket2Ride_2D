using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    int _highestCheckpointID = -1;
    Vector3 _checkpointPosition;


    [SerializeField] private List<GameObject> objectsToReset = new List<GameObject>();
    [SerializeField] private GameObject Door;
    [SerializeField] private GameObject Lever;
    private List<Vector3> originalPositions = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (GameObject obj in objectsToReset)
            {
                if (obj != null)
                    originalPositions.Add(obj.transform.position);
            }
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

    public void ResetSceneObjects()
{
    for (int i = 0; i < objectsToReset.Count; i++)
    {
        GameObject obj = objectsToReset[i];

        // Skip if the object is destroyed (null)
        if (obj == null)
        {
            Debug.LogWarning($"Object at index {i} is missing (destroyed?) and will be skipped.");
            continue; // Skip destroyed objects
        }

        Vector3 originalPos = originalPositions[i];

        // Reset position and rotation to the initial state
        obj.transform.position = originalPos;
        obj.transform.rotation = Quaternion.identity;

        // Reset physics
        if (obj.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    // Reset the Door animation (if any)
    if (Door != null && Door.TryGetComponent<Animator>(out Animator animatorDoor))
    {
        animatorDoor.SetTrigger("Close");
    }

    // Reset the Lever animation (if any)
    if (Lever != null && Lever.TryGetComponent<Animator>(out Animator animatorLever))
    {
        animatorLever.SetBool("isActive", false);
    }
}


    public void ResetCheckpoints()
    {
        _highestCheckpointID  = -1;
        _checkpointPosition = Vector3.zero;
    }
}
