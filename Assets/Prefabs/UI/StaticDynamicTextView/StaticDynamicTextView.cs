using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component to display a static text along with a dynamic text that can be updated during runtime.
/// </summary>
public class StaticDynamicTextView : MonoBehaviour
{
    [Header("TextView Settings")]
    [Tooltip("The static text that remains constant.")]
    public string staticText;

    [Tooltip("Reference to the UI Text component displaying the static text.")]
    public Text textStatic;

    [Tooltip("Reference to the UI Text component displaying the dynamic text.")]
    public Text textDynamic;

    // The dynamic text that can be updated during runtime.
    private string dynamicText;

    // Called in the editor when a value is changed.
    void OnValidate()
    {
        if (textStatic != null && textStatic.text != null)
        {
            textStatic.text = staticText;
        }
    }

    // Start is called before the first frame update.
    void Start()
    {
        // Set the static text at the start.
        textStatic.text = staticText;

        // Update dynamic text if available.
        UpdateDynamicText();
    }

    /// <summary>
    /// Property to get or set the dynamic text.
    /// </summary>
    public string DynamicText
    {
        get { return dynamicText; }
        set
        {
            dynamicText = value;
            UpdateDynamicText();
        }
    }

    // Update the display of dynamic text.
    private void UpdateDynamicText()
    {
        if (textDynamic != null)
            textDynamic.text = dynamicText;
    }
}
