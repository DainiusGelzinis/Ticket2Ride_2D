using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{   
    public Slider slider;
    public float autoSpeed = 0.2f; // How fast the slider auto-moves (units/sec)
    public float minSliderValue = -5f;
    public float maxSliderValue = 5f;

    [SerializeField] private PlayerController playerController;

    public float currentInertiaSpeed;

    private bool isDragging = false;

    private void Start()
    {
        slider.minValue = minSliderValue;
        slider.maxValue = maxSliderValue;
        slider.value = minSliderValue;
    }

    private void Update()
    {   

        float value = slider.value;
        float inertia = 0;
        float SliderPosition = (value - minSliderValue) / (maxSliderValue - minSliderValue);

        if (!isDragging)
        {
            slider.value += autoSpeed * Time.deltaTime;
            if (SliderPosition >= 1)
            {
                slider.value = minSliderValue;
            }
        }


        //slider position is between 0 and 1
        if (SliderPosition < 0.25f)
            inertia = 5;   //pushed to the left
        else if (SliderPosition >= 0.25f && SliderPosition <= 0.75f)
            inertia = 0;    // neutral zone
        else if (SliderPosition > 0.75f)
            inertia = -5;   // pushed to the right

        // Tell the PlayerController about it:  
        playerController.SetInertiaForce(inertia);

        
    }

    // Detect manual drag
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
