using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Joystick : MonoBehaviour
{
    [Header("Joystick Handle Settings")]
    public Transform objectToMove;
    public float speed = 6.0f;
    public bool backToCenterWhenRealsed = true;

    public enum Axes
    {
        X,
        Y,
        Z
    }

    // Variable to let the choice of the axis.
    public Axes firstAxes = Axes.X;
    public Axes secondAxes = Axes.Y;

    public enum TransformVariables
    {
        Position,
        Rotation
    }

    public TransformVariables transformModifiedValue = TransformVariables.Position;

    public UnityEvent<float> onFirstAxisValueChange;
    public UnityEvent<float> onSecondAxisValueChange;

    private JoystickHandle joystickHandle;

    public void Awake()
    {
        joystickHandle = gameObject.GetComponentInChildren<JoystickHandle>();
    }

    public void Start()
    {
        changeJoystickHandleValues();
    }
    
    public void OnValidate()
    {
        changeJoystickHandleValues();
    }

    private void changeJoystickHandleValues()
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
