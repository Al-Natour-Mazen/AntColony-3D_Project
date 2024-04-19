using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages a progress bar UI element.
/// </summary>
public class ProgressBar : MonoBehaviour
{
    private Slider slider; // Reference to the slider component

    [Header("Progress Bar Settings")]
    [Tooltip("The maximum value the progress bar can reach.")]
    public float maxValue = 100; // Maximum value of the progress bar

    [Tooltip("The current value of the progress bar.")]
    public float currentValue = 0; // Current value of the progress bar

    [Tooltip("The current color of the filled portion of the progress bar.")]
    public Color fillColor = Color.green; // color of the filled portion of the progress bar.

    void Start()
    {
        // Get the Slider component attached to this GameObject
        slider = GetComponent<Slider>();
        slider.maxValue = maxValue;
        slider.value = currentValue;

        // Find the Fill Image within the slider and set its color
        Transform fill = slider.transform.Find("Fill Area/Fill");
        if (fill != null)
        {
            Image fillImage = fill.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = fillColor;
            }
        }
    }

    void Update()
    {
        // For demonstration purposes, advancing the progress by a fixed amount each frame
        //AdvanceProgress(0.01F);
    }

    /// <summary>
    /// Advances the progress of the bar by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to advance the progress by.</param>
    public void AdvanceProgress(float amount)
    {
        currentValue = amount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        slider.value = currentValue;
    }

    /// <summary>
    /// Returns the current value of the progress bar.
    /// </summary>
    /// <returns>The current value of the progress bar.</returns>
    public float GetCurrentValue()
    {
        return currentValue;
    }

    /// <summary>
    /// Sets the current value of the progress bar.
    /// </summary>
    /// <param name="value">The value to set as the current value.</param>
    public void SetCurrentValue(float value)
    {
        currentValue = value;
        slider.value = currentValue;
    }

    /// <summary>
    /// Returns the maximum value of the progress bar.
    /// </summary>
    /// <returns>The maximum value of the progress bar.</returns>
    public float GetMaxValue()
    {
        return maxValue;
    }

    /// <summary>
    /// Sets the maximum value of the progress bar.
    /// </summary>
    /// <param name="value">The value to set as the maximum value.</param>
    public void SetMaxValue(float value)
    {
        maxValue = value;
        slider.maxValue = maxValue;
    }
}
