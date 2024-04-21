using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a text view input field that allows validation of input data types.
/// </summary>
public class TextViewInputField : MonoBehaviour
{
    [Header("TextView Settings")]
    [Tooltip("The static text that remains constant.")]
    public string staticText;

    [Tooltip("The background image for the input field.")]
    public Sprite backgroundInputField;

    [Tooltip("The text component displaying static text.")]
    public Text textStatic;

    [Tooltip("The input field component.")]
    public InputField inputField;

    /// <summary>
    /// The data type expected for input validation.
    /// </summary>
    public enum DataType
    {
        String,
        Integer,
        Float
        // Add more data types if needed
    }

    [Tooltip("The data type expected for input validation.")]
    public DataType dataType;

    /// <summary>
    /// Unity method called in the editor when a value is changed.
    /// </summary>
    void OnValidate()
    {
        if (textStatic != null && textStatic.text != null)
        {
            textStatic.text = staticText;
        }

        if (inputField != null)
        {
            inputField.GetComponent<Image>().sprite = backgroundInputField;
        }
    }

    /// <summary>
    /// Unity method called on start.
    /// </summary>
    void Start()
    {
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    /// <summary>
    /// Validates input based on the selected data type.
    /// </summary>
    void OnInputValueChanged(string value)
    {
        switch (dataType)
        {
            case DataType.String:
                // No validation needed for string
                break;
            case DataType.Integer:
                int intValue;
                if (!int.TryParse(value, out intValue))
                {
                    inputField.text = ""; // Clear input if not valid integer
                }
                break;
            case DataType.Float:
                float floatValue;
                if (!float.TryParse(value, out floatValue))
                {
                    inputField.text = ""; // Clear input if not valid float
                }
                break;
                // Add cases for more data types if needed
        }
    }

    /// <summary>
    /// Gets the content of the InputField.
    /// </summary>
    /// <returns>The content of the InputField as a string.</returns>
    public string GetInputFieldValue()
    {
        return inputField.text;
    }

    /// <summary>
    /// Sets the content of the InputField.
    /// </summary>
    public void SetInputFieldValue(string value)
    {
        inputField.text = value;
    }
}
