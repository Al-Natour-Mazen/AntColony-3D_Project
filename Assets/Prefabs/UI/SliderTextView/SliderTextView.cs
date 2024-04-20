using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates a text component based on the value of a slider.
/// </summary>
public class SliderTextView : MonoBehaviour
{
    [Header("Slider Settings")]
    public int MaxValue;
    public int MinValue;

    private Slider mySlider;
    private Text myText;

    /// <summary>
    /// Initializes the slider and text components.
    /// </summary>
    void Start()
    {
        // Get the children of the GameObject attached to this script
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Slider>() != null)
            {
                mySlider = child.GetComponent<Slider>();
                mySlider.maxValue = MaxValue;
                mySlider.minValue = MinValue;
                // Add a listener to detect changes in the Slider value
                mySlider.onValueChanged.AddListener(OnSliderValueChanged);
            }
            else if (child.GetComponent<Text>() != null)
            {
                myText = child.GetComponent<Text>();
                myText.text = MinValue.ToString();
            }
        }
    }

    /// <summary>
    /// Called when the value of the Slider changes.
    /// </summary>
    /// <param name="value">The new value of the Slider.</param>
    void OnSliderValueChanged(float value)
    {
        if (myText != null)
        {
            myText.text = value.ToString("0.0");
        }
    }

    /// <summary>
    /// Gets the maximum value of the Slider.
    /// </summary>
    /// <returns>The maximum value of the Slider.</returns>
    public int GetMaxValue()
    {
        return MaxValue;
    }

    /// <summary>
    /// Gets the minimum value of the Slider.
    /// </summary>
    /// <returns>The minimum value of the Slider.</returns>
    public int GetMinValue()
    {
        return MinValue;
    }

    /// <summary>
    /// Gets the current value of the Slider.
    /// </summary>
    /// <returns>The current value of the Slider.</returns>
    public float GetCurrentValue()
    {
        if (mySlider != null)
        {
            return mySlider.value;
        }
        return 0f;
    }
}
