using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Slider Settings")]
    public Slider slider;
    public float autoSpeed = 0.2f; // How fast the slider auto‐moves (units/sec)
    public float minSliderValue = -5f;
    public float maxSliderValue = 5f;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    // (Optional) If you ever want to let PlayerController stop this slider on death:
    // private bool isPaused = false;

    private bool isDragging = false;

    private void Start()
    {
        // Initialize min/max and starting value
        slider.minValue = minSliderValue;
        slider.maxValue = maxSliderValue;
        slider.value = minSliderValue;
    }

    private void Update()
    {
        // If you ever need to pause the slider when the player dies,
        // uncomment the next two lines and let PlayerController set isPaused = true on death:
        //
        // if (isPaused)
        //     return;

        // Auto‐move the slider when not being dragged
        if (!isDragging)
        {
            slider.value += autoSpeed * Time.deltaTime;

            float normalizedPos = (slider.value - minSliderValue) / (maxSliderValue - minSliderValue);
            if (normalizedPos >= 1f)
            {
                slider.value = minSliderValue;
            }
        }

        // Compute normalized slider position (0 .. 1)
        float sliderValue = slider.value;
        float sliderPositionNormalized = (sliderValue - minSliderValue) / (maxSliderValue - minSliderValue);

        // Determine inertia based on where the slider is
        float inertia = 0f;
        if (sliderPositionNormalized < 0.25f)
        {
            inertia = 5f;   // pushed to the left
        }
        else if (sliderPositionNormalized > 0.75f)
        {
            inertia = -5f;  // pushed to the right
        }
        else
        {
            inertia = 0f;   // neutral zone
        }

        // Send the current inertia value to PlayerController
        playerController.SetInertiaForce(inertia);
    }

    // Called when the user clicks or taps on the Slider handle
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    // Called when the user releases the click/tap
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    // (Optional) If PlayerController wants to pause the slider when the player dies:
    // public void PauseSlider()
    // {
    //     isPaused = true;
    // }
}

