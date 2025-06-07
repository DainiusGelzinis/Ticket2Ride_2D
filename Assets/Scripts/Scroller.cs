using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Scroller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;

    [Header("Speed Settings")]
    [Tooltip("Starting scroll speed in units/sec (positive to the right, negative to the left)")]
    [SerializeField] private float initialSpeed = 1f;

    [Tooltip("Acceleration factor: how strongly inertia shifts speed per tick")]
    [SerializeField] private float speedFactor = 0.05f;

    private float currentSpeed;
    private float offset;
    private Material mat;

    private void Start()
    {
        // Grab the material instance
        mat = GetComponent<Renderer>().material;

        // Initialize
        currentSpeed = initialSpeed;

        // Kick off the “every 0.1s” speed adjustment
        StartCoroutine(SpeedAdjustLoop());
    }

    private void Update()
    {
        // Scroll the texture every frame by currentSpeed
        offset += currentSpeed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0f));
    }

    private IEnumerator SpeedAdjustLoop()
    {
        // This loop runs forever; every 0.1s it tweaks currentSpeed
        while (true)
        {
            // Read inertia from the slider/controller
            float inertia = playerController.GetInertiaForce();

            // Adjust speed by inertia, small step of 0.01 * speedFactor
            currentSpeed += inertia * 0.01f * speedFactor;

            // Wait exactly 0.1 seconds before the next adjustment
            yield return new WaitForSeconds(0.1f);
        }
    }
}
