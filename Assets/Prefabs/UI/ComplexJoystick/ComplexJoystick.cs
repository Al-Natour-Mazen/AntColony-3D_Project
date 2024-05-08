using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages a complex joystick UI element for controlling object movement.
/// </summary>
public class ComplexJoystick : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject secondAxisContainer; // Container for the second axis UI elements

    [Header("Object Settings")]
    public Transform objectToMove; // Object to be moved

    [Header("Default Values")]
    public Vector3 defaultPosition; // Default position of the object
    public Vector3 defaultRotation; // Default rotation of the object

    [Header("Joystick Settings")]
    public Joystick.Axes firstAxes = Joystick.Axes.X; // First joystick axis
    public Joystick.Axes secondAxes = Joystick.Axes.Y; // Second joystick axis
    public Joystick.TransformVariables transformModifiedValue = Joystick.TransformVariables.Position; // Transform modified by the joystick

    private Joystick joystick; // Reference to the main joystick component
    private JoystickHandle joystickHandle; // Reference to the joystick handle component

    private Text firstAxisText; // Text displaying the first axis label
    private Text secondAxisText; // Text displaying the second axis label

    private Text firstAxisValueText; // Text displaying the value of the first axis
    private Text secondAxisValueText; // Text displaying the value of the second axis

    private Button resetButton; // Button for resetting the object's position and rotation

    public void Awake()
    {
        joystick = gameObject.GetComponentInChildren<Joystick>(); // Get the main joystick component
        joystickHandle = gameObject.GetComponentInChildren<JoystickHandle>(); // Get the joystick handle component

        Text[] t = gameObject.GetComponentsInChildren<Text>(); // Get all Text components in children
        firstAxisText = t[0]; // First Text component is the first axis label
        firstAxisValueText = t[1]; // Second Text component is the value of the first axis
        secondAxisText = t[2]; // Third Text component is the second axis label
        secondAxisValueText = t[3]; // Fourth Text component is the value of the second axis

        resetButton = gameObject.GetComponentInChildren<Button>(); // Get the reset button component
    }

    public void Start()
    {
        // Set object to move for joystick
        if (joystick != null)
        {
            joystick.objectToMove = objectToMove;
            joystick.onFirstAxisValueChange.AddListener(changeFirstValueText);
            joystick.onSecondAxisValueChange.AddListener(changeSecondValueText);
            changeJoystickValues();
        }

        // Set reset button listener
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(reset);
        }
    }
    
    public void OnValidate()
    {
        changeJoystickValues();
    }

    private void changeJoystickValues()
    {
        // Update joystick settings and UI labels
        if (joystick != null)
        {
            joystick.firstAxes = firstAxes;
            joystick.secondAxes = secondAxes;
            joystick.transformModifiedValue = transformModifiedValue;
            firstAxisText.text = firstAxes.ToString() + " :";
            secondAxisText.text = secondAxes.ToString() + " :";

            bool activeSecondContainer = firstAxes != secondAxes;
            secondAxisContainer.SetActive(activeSecondContainer);
            joystick.OnValidate();
        }
    }

    private void changeFirstValueText(float v)
    {
        // Update text displaying the value of the first axis
        if (firstAxisValueText != null)
        {
            firstAxisValueText.text = v.ToString("0.00");
        }
    }

    private void changeSecondValueText(float v)
    {
        // Update text displaying the value of the second axis
        if (secondAxisValueText != null)
        {
            secondAxisValueText.text = v.ToString("0.00");
        }
    }

    private void reset()
    {
        // Reset the object's position and rotation
        objectToMove.localPosition = defaultPosition;
        objectToMove.localEulerAngles = defaultRotation;
        joystickHandle.MoveObject(new Vector2(0.0f, 0.0f));
    }
}