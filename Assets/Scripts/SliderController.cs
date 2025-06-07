using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Slider Settings")]
    public Slider slider;
    public float autoSpeed = 0.2f;       // How fast the slider auto‐moves (units/sec)
    public float minSliderValue = -5f;
    public float maxSliderValue = 5f;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    // Smoothed inertia state
    private float _smoothedInertia = 0f;
    private Coroutine _inertiaRoutine;

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
        // Auto‐move the slider when not being dragged
        if (!isDragging)
        {
            slider.value += autoSpeed * Time.deltaTime;
            float normalizedPos = (slider.value - minSliderValue) / (maxSliderValue - minSliderValue);
            if (normalizedPos >= 1f)
                slider.value = minSliderValue;
        }

        // Compute normalized slider position (0 .. 1)
        float sliderPositionNormalized = (slider.value - minSliderValue) / (maxSliderValue - minSliderValue);

        // Determine raw inertia based on slider zones
        float rawInertia;
        
        if (sliderPositionNormalized < 0.08f)
            rawInertia = 0f;  // pushed to the far left
        else if (sliderPositionNormalized < 0.4f && sliderPositionNormalized > 0.08f)
            rawInertia = 5f;   // pushed to the left
        else if (sliderPositionNormalized >= 0.4f && sliderPositionNormalized <= 0.6f)
            rawInertia = 0f;   // neutral zone
        else if (sliderPositionNormalized > 0.6f && sliderPositionNormalized < 0.9f)
            rawInertia = -5f;   // pushed to the right
        else if (sliderPositionNormalized > 0.90f)
            rawInertia = 0f;   // pushed to the far right
        else
            rawInertia = 0f;   // neutral zone


        // Smoothly ramp _smoothedInertia toward rawInertia over 2 seconds
        ChangeInertiaSmoothly(rawInertia);

        // Send the smoothed inertia value to PlayerController
        playerController.SetInertiaForce(_smoothedInertia);
    }

    
    public void ChangeInertiaSmoothly(float targetInertia)
    {
        if (_inertiaRoutine != null)
            StopCoroutine(_inertiaRoutine);

        _inertiaRoutine = StartCoroutine(InertiaTweenCoroutine(targetInertia));
    }

    private IEnumerator InertiaTweenCoroutine(float target)
    {
        float start    = _smoothedInertia;
        float duration = 1f;
        float t        = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            _smoothedInertia = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }

        // Ensure we hit the exact target at the end
        _smoothedInertia = target;
        _inertiaRoutine  = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
