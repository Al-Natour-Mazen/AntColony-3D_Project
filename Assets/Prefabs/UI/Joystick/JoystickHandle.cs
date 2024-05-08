using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Handles the movement of an object using a joystick input.
/// </summary>
public class JoystickHandle : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform objectToMove; // The object to be moved.
    public float speed; // The speed of movement.
    public bool backToCenterWhenRealsed; // Whether the joystick should return to center when released.

    private Vector2 startPoint; // The starting point of joystick movement.
    private Vector2 endPoint; // The end point of joystick movement.
    private Vector2 direction; // The direction of joystick movement.
    private Vector3 initialPoint; // The initial position of the joystick handle.

    private bool mouseDown = false; // Flag indicating if the mouse button is pressed.

    private Joystick.Axes firstAxis; // The primary axis of movement.
    private Joystick.Axes secondAxis; // The secondary axis of movement.
    private Joystick.TransformVariables transformVariables; // The type of transformation to apply.

    private UnityEvent<float> onFirstAxisValueChange; // Event invoked when the value of the first axis changes.
    private UnityEvent<float> onSecondAxisValueChange; // Event invoked when the value of the second axis changes.

    /// <summary>
    /// Initializes the initial position and direction.
    /// </summary>
    public void Start()
    {
        initialPoint = transform.localPosition;
        direction = new Vector2(0.0f, 0.0f);
    }

    /// <summary>
    /// Called when the mouse button is pressed.
    /// </summary>
    public void OnMouseDown()
    {
        startPoint = Input.mousePosition;
    }
    
    /// <summary>
    /// Called when the mouse button is dragged.
    /// </summary>
    public void OnMouseDrag()
    {
        mouseDown = true;
        endPoint = Input.mousePosition;
    }

    /// <summary>
    /// Called when the mouse button is released.
    /// </summary>
    public void OnMouseUp()
    {
        mouseDown = false;
        if(backToCenterWhenRealsed)
        { 
            transform.localPosition = initialPoint;
        }
    }

    /// <summary>
    /// Updates the movement of the joystick handle and the object.
    /// </summary>
    public void Update()
    {
        if(mouseDown)
        {   
            Vector2 offset = endPoint - startPoint;
            direction = Vector2.ClampMagnitude(offset, 25.0f);
            transform.localPosition = new Vector3(direction.x, direction.y, 0.0f);
        }
        if(!backToCenterWhenRealsed || mouseDown)
        {
            MoveObject(direction);
        }
    }

    /// <summary>
    /// Moves the specified object based on the joystick input.
    /// </summary>
    /// <param name="direction">The direction of movement.</param>
    public void MoveObject(Vector2 direction)
    {
        if(objectToMove != null)
        {
            Vector3 axes = new Vector3(0.0f, 0.0f, 0.0f);
            affectValueToAxes(secondAxis, ref axes, direction.y);
            affectValueToAxes(firstAxis, ref axes, direction.x);

            axes = axes * Time.deltaTime * speed;

            Vector3 newValues;

            if(transformVariables == Joystick.TransformVariables.Position)
            {
                objectToMove.Translate(axes.x, axes.y, axes.z);
                newValues = objectToMove.localPosition;
            }
            else // Joystick.TransformVariables.Rotation
            {
                objectToMove.Rotate(axes.x, axes.y, axes.z);
                newValues = objectToMove.localEulerAngles;
            }

            float firstV = updateAxesValues(firstAxis, newValues);
            float secondV = updateAxesValues(secondAxis, newValues);
            updateAxesValues(firstV, secondV);
        }
    }

    /// <summary>
    /// Assigns the joystick input value to the corresponding axis.
    /// </summary>
    private void affectValueToAxes(Joystick.Axes axis, ref Vector3 vecAxes, float value)
    {
        switch(axis)
        {
            case Joystick.Axes.X:
                vecAxes.x = value;
                break;
            case Joystick.Axes.Y:
                vecAxes.y = value;
                break;
            case Joystick.Axes.Z:
                vecAxes.z = value;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Updates the axis values based on the object's transformation.
    /// </summary>
    private float updateAxesValues(Joystick.Axes axis, Vector3 vecValues)
    {
        switch(axis)
        {
            case Joystick.Axes.X:
                return vecValues.x;
            case Joystick.Axes.Y:
                return vecValues.y;
            case Joystick.Axes.Z:
                return vecValues.z;
            default:
                return 0.0f;
        }
    }

    /// <summary>
    /// Invokes events with the updated axis values.
    /// </summary>
    private void updateAxesValues(float firstAxis, float secondAxis)
    {
        if(onFirstAxisValueChange != null)
        {
            onFirstAxisValueChange.Invoke(firstAxis);
        }
        if(onSecondAxisValueChange != null)
        {
            onSecondAxisValueChange.Invoke(secondAxis);
        }
    }

    /// <summary>
    /// Sets the object to be moved by the joystick.
    /// </summary>
    /// <param name="p">The object to be moved.</param>
    public void setPlayer(Transform p)
    {
        objectToMove = p;
    }

    /// <summary>
    /// Sets the speed of movement.
    /// </summary>
    /// <param name="s">The speed value.</param>
    public void setSpeed(float s)
    {
        speed = s;
    }

    /// <summary>
    /// Sets whether the joystick should return to center when released.
    /// </summary>
    /// <param name="b">True if joystick should return to center, false otherwise.</param>
    public void setBackToCenterWhenRealsed(bool b)
    {
        backToCenterWhenRealsed = b;
    }

    /// <summary>
    /// Sets the axes of movement.
    /// </summary>
    /// <param name="firstA">The primary axis.</param>
    /// <param name="secondA">The secondary axis.</param>
    public void setAxes(Joystick.Axes firstA, Joystick.Axes secondA)
    {
        firstAxis = firstA;
        secondAxis = secondA;
    }

    /// <summary>
    /// Sets the type of transformation to apply.
    /// </summary>
    /// <param name="transformV">The transformation type.</param>
    public void setTransformVariables(Joystick.TransformVariables transformV)
    {
        transformVariables = transformV;
    }
    
    /// <summary>
    /// Sets the event to be invoked when the value of the first axis changes.
    /// </summary>
    /// <param name="e">The UnityEvent containing float parameter.</param>
    public void setOnFirstAxisValueChange(UnityEvent<float> e)
    {
        onFirstAxisValueChange = e;
    }

    /// <summary>
    /// Sets the event to be invoked when the value of the second axis changes.
    /// </summary>
    /// <param name="e">The UnityEvent containing float parameter.</param>
    public void setOnSecondAxisValueChange(UnityEvent<float> e)
    {
        onSecondAxisValueChange = e;
    }
}