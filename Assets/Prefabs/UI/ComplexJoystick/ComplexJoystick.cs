using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComplexJoystick : MonoBehaviour
{
    public GameObject secondAxisContainer;

    public Transform objectToMove;

    public Vector3 defaultPosition;
    public Vector3 defaultRotation;

    public Joystick.Axes firstAxes = Joystick.Axes.X;
    public Joystick.Axes secondAxes = Joystick.Axes.Y;
    public Joystick.TransformVariables transformModifiedValue = Joystick.TransformVariables.Position;

    private Joystick joystick;
    private JoystickHandle joystickHandle;

    private Text firstAxisText;
    private Text secondAxisText;

    private Text firstAxisValueText;  
    private Text secondAxisValueText;

    private Button resetButton;

    public void Awake()
    {
        joystick = gameObject.GetComponentInChildren<Joystick>();
        joystickHandle = gameObject.GetComponentInChildren<JoystickHandle>();

        Text[] t = gameObject.GetComponentsInChildren<Text>();
        firstAxisText = t[0];
        firstAxisValueText = t[1];
        secondAxisText = t[2];
        secondAxisValueText = t[3];

        resetButton = gameObject.GetComponentInChildren<Button>();
    }


    public void Start()
    {
        
        if(joystick != null)
        {
            joystick.objectToMove = objectToMove;
            joystick.onFirstAxisValueChange.AddListener(changeFirstValueText);
            joystick.onSecondAxisValueChange.AddListener(changeSecondValueText);
            changeJoystickValues();
        }
        if(resetButton != null)
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
        if(joystick != null)
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
        if(firstAxisValueText != null)
        {
            firstAxisValueText.text = v.ToString("0.00");
        }
    }

    private void changeSecondValueText(float v)
    {
        if(secondAxisValueText != null)
        {
            secondAxisValueText.text = v.ToString("0.00");
        }
    }

    private void reset()
    {
        objectToMove.localPosition = defaultPosition;
        objectToMove.localEulerAngles = defaultRotation;
        joystickHandle.MoveObject(new Vector2(0.0f, 0.0f));
    }


}
