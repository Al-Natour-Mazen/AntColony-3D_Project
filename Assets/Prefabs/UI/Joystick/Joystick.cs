using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Main class responsible for managing the joystick functionality.
/// </summary>
public class Joystick : MonoBehaviour
{
    [Header("Joystick Handle Settings")]
    public Transform objectToMove; // The object to be moved.
    public float speed = 6.0f; // The speed of movement.
    public bool backToCenterWhenRealsed = true; // Whether the joystick should return to center when released.

    public enum Axes
    {
        X,
        Y,
        Z
    }

    // Variable to let the choice of the axis.
    public Axes firstAxes = Axes.X; // The primary axis of movement.
    public Axes secondAxes = Axes.Y; // The secondary axis of movement.

    public enum TransformVariables
    {
        Position,
        Rotation
    }

    public TransformVariables transformModifiedValue = TransformVariables.Position; // The type of transformation to apply.

    public UnityEvent<float> onFirstAxisValueChange; // Event invoked when the value of the first axis changes.
    public UnityEvent<float> onSecondAxisValueChange; // Event invoked when the value of the second axis changes.

    private JoystickHandle joystickHandle; // Reference to the JoystickHandle component.

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        joystickHandle = gameObject.GetComponentInChildren<JoystickHandle>();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        ChangeJoystickHandleValues();
    }
    
    /// <summary>
    /// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    /// </summary>
    public void OnValidate()
    {
        ChangeJoystickHandleValues();
    }

    /// <summary>
    /// Changes the values of the attached JoystickHandle component based on the settings of this class.
    /// </summary>
    private void ChangeJoystickHandleValues()
    {
        if(joystickHandle != null)
        {
            joystickHandle.setPlayer(objectToMove);
            joystickHandle.setSpeed(speed);
            joystickHandle.setBackToCenterWhenRealsed(backToCenterWhenRealsed);
            joystickHandle.setAxes(firstAxes, secondAxes);
            joystickHandle.setTransformVariables(transformModifiedValue);
            joystickHandle.setOnFirstAxisValueChange(onFirstAxisValueChange);
            joystickHandle.setOnSecondAxisValueChange(onSecondAxisValueChange);
        }
    }
}
