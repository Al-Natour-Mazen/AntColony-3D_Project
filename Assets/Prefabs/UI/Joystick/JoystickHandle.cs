using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class JoystickHandle : MonoBehaviour
{
    private Transform objectToMove;
    private float speed;
    private bool backToCenterWhenRealsed;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 direction;
    private Vector3 initialPoint;

    private bool mouseDown = false;

    private Joystick.Axes firstAxis;
    private Joystick.Axes secondAxis;
    private Joystick.TransformVariables transformVariables;

    private UnityEvent<float> onFirstAxisValueChange;
    private UnityEvent<float> onSecondAxisValueChange;

    public void Start()
    {
        initialPoint = transform.localPosition;
        direction = new Vector2(0.0f, 0.0f);
    }

    public void OnMouseDown()
    {
        startPoint = Input.mousePosition;
    }
    
    public void OnMouseDrag()
    {
        mouseDown = true;
        endPoint = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        mouseDown = false;
        if(backToCenterWhenRealsed)
        { 
            transform.localPosition = initialPoint;
        }
    }

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

    public void setPlayer(Transform p)
    {
        objectToMove = p;
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    public void setBackToCenterWhenRealsed(bool b)
    {
        backToCenterWhenRealsed = b;
    }

    public void setAxes(Joystick.Axes firstA, Joystick.Axes secondA)
    {
        firstAxis = firstA;
        secondAxis = secondA;
    }

    public void setTransformVariables(Joystick.TransformVariables transformV)
    {
        transformVariables = transformV;
    }
    
    public void setOnFirstAxisValueChange(UnityEvent<float> e)
    {
        onFirstAxisValueChange = e;
    }

    public void setOnSecondAxisValueChange(UnityEvent<float> e)
    {
        onSecondAxisValueChange = e;
    }
}
