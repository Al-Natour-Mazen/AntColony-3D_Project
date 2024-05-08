using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AntColonyPersistenceManager;
using System;

public class SimulationController : MonoBehaviour
{
    [Header("Board Settings")]
    public Board board;
    public int width = 20;
    public int height = 20;

    [Header("Entity Counts")]
    public int antCount = 15;
    public int wallCountDensity = 80;
    public int seedCountDensity = 57;
    public int maxSeedQuantityPerBlock = 10;

    [Header("UI Settings")]
    public ProgressBar progressBar;
    public PlayPauseButton playPauseButton;
    public SliderTextView sliderTextView;
    public StaticDynamicTextView dynamicTextViewAnt;
    public StaticDynamicTextView dynamicTextViewTime;
    public StaticDynamicTextView dynamicTextViewSeedInColony;
    public StaticDynamicTextView dynamicTextViewSeedOutColony;
    public TextViewInputField inputfieldAnt;
    public TextViewInputField inputfieldWallDensity;
    public TextViewInputField inputfieldSeedDensity;
    public TextViewInputField inputfieldSeedPerBlock;
    public TextViewInputField inputfieldWidthSimulation;
    public TextViewInputField inputfieldHeightSimulation;

    public Menu menu;
    public ColonyCamera colonyCamera;
    public FPSCamera FPSCamera;
    public Transform mainCamera;

    private float lastTime;
    private AntConlonySimulation antSimulation;
    private float simulationSpeed = 1.0f;
    private float elapsedTime = 0f;
    private int MaximalPossibleSeedInColony;

    void Start()
    {
        InitializeSimulation();
    }

    private void InitializeSimulation()
    {
        elapsedTime = 0F;
        antSimulation = new AntConlonySimulation(width, height, maxSeedQuantityPerBlock, wallCountDensity, antCount, seedCountDensity);
        InitializeBoard();
        StartCoroutine(InitializeUIComponents()); 
    }

    private void InitializeBoard()
    {
        board.SetHeightBoard(height);
        board.SetWidthBoard(width);
        board.InstantiateAnthill(antSimulation);
        board.GenerateWalls(antSimulation);
        board.GenerateAnts(antSimulation);
        board.GenerateSeeds(antSimulation);
    }

    private IEnumerator InitializeUIComponents()
    {
        // Wait for the end of the frame. This ensures that all other Start and Update methods have been called for this frame.
        // This is useful when we want to make sure that all other objects and their scripts have been initialized before using them.
        yield return new WaitForEndOfFrame();

        // Now that we are sure everything is initialized, we can set our components.
        playPauseButton.EnableDiableButton(true);
        MaximalPossibleSeedInColony = antSimulation.GetTotalSeedOutColony();
        progressBar.SetMaxValue(MaximalPossibleSeedInColony);
        UpdateUI();
        UpdateInputFields();
        InitCamera();
    }

    private void InitCamera()
    {
        initColonyCamera();
        initFPSCamera();
        initMainCamera();
    }

    private void UpdateUI()
    {
        simulationSpeed = sliderTextView.GetCurrentValue();
        dynamicTextViewTime.DynamicText = FormatTime(elapsedTime);
        dynamicTextViewAnt.DynamicText = antSimulation.GetAntsInColony().Count.ToString();
        dynamicTextViewSeedInColony.DynamicText = antSimulation.GetTotalSeedInColony().ToString();
        dynamicTextViewSeedOutColony.DynamicText = antSimulation.GetTotalSeedOutColony().ToString();
    }
       
    private void UpdateInputFields()
    {
        inputfieldAnt.SetInputFieldValue(antCount.ToString());
        inputfieldWallDensity.SetInputFieldValue(wallCountDensity.ToString());
        inputfieldSeedDensity.SetInputFieldValue(seedCountDensity.ToString());
        inputfieldSeedPerBlock.SetInputFieldValue(maxSeedQuantityPerBlock.ToString());
        inputfieldWidthSimulation.SetInputFieldValue(width.ToString());
        inputfieldHeightSimulation.SetInputFieldValue(height.ToString());
    }


    void Update()
    {
        float currentTime = Time.fixedTime;
        float sliderValue = sliderTextView.GetCurrentValue();

        UpdateSimulationSpeed(sliderValue, currentTime);

        if (playPauseButton.IsPlaying())
        {
            if (simulationSpeed <= (currentTime - lastTime))
            {
                UpdateSimulation(currentTime);
            }

            elapsedTime += Time.deltaTime * sliderValue;
            dynamicTextViewTime.DynamicText = FormatTime(elapsedTime);
            // We chack if the Simulation is End
            if (antSimulation.GetTotalSeedOutColony() == 0 && antSimulation.GetTotalSeedInColony() == MaximalPossibleSeedInColony)
            {
                SaveSimulation();
                playPauseButton.ChangeButtonPlayingState();
                playPauseButton.EnableDiableButton(false);
            }
        }
        else
        {
            menu.EnableButton();
        }

      
    }

    private void UpdateSimulationSpeed(float sliderValue, float currentTime)
    {
        if (sliderValue <= 0)
        {
            simulationSpeed = 1.0f;
        }
        else
        {
            simulationSpeed = (1.2f / (sliderValue + 0.001f * 100));
        }
    }

    private void UpdateSimulation(float currentTime)
    {
        menu.DisableButton();
        antSimulation.EvolveTheAntColony();
        UpdateUI();
        progressBar.AdvanceProgress(antSimulation.GetTotalSeedInColony());
        board.GenerateAnts(antSimulation);
        board.GenerateSeeds(antSimulation);
        lastTime = currentTime;
    }

    private string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void InitOtherEnvironement()
    {
        if (TryParseInt(inputfieldAnt.GetInputFieldValue(), out int numberOfAnt) &&
            TryParseInt(inputfieldWallDensity.GetInputFieldValue(), out int wallDensity) &&
            TryParseInt(inputfieldSeedDensity.GetInputFieldValue(), out int seedDensity) &&
            TryParseInt(inputfieldSeedPerBlock.GetInputFieldValue(), out int maxSeedBlock))
        {
            antCount = numberOfAnt;
            wallCountDensity = wallDensity;
            seedCountDensity = seedDensity;
            maxSeedQuantityPerBlock = maxSeedBlock;
            InitializeSimulation();
        }
        else
        {
            Debug.Log("Error in InitOtherEnvironement Function");
        }
    }

    public void InitOtherDimension()
    {
        if (TryParseInt(inputfieldWidthSimulation.GetInputFieldValue(), out int width) &&
            TryParseInt(inputfieldHeightSimulation.GetInputFieldValue(), out int height))
        {
            int GapAroundHill = AntConlonySimulation.GetGapAroundHill();
            int minDim = GapAroundHill * 2;
            if (width < minDim)
            {
                width = minDim;
                inputfieldWidthSimulation.SetInputFieldValue("" + minDim);
            }
            if (height < minDim)
            {
                height = minDim;
                inputfieldHeightSimulation.SetInputFieldValue("" + minDim);
            }
            this.width = width;
            this.height = height;
            InitializeSimulation();
        }
        else
        {
            Debug.Log("Error in InitOtherEnvironement Function");
        }
    }

    private bool TryParseInt(string input, out int result)
    {
        return int.TryParse(input, out result);
    }

    public void SaveSimulation()
    {
        AntColonyPersistenceManager.SaveColonyInfo(antSimulation);
    }

    private void initFPSCamera()
    {
        if(FPSCamera != null)
        {
            int offset =  1 + 10;
            FPSCamera.maxX = antSimulation.GetWidthSimulation() + offset;
            FPSCamera.maxZ = antSimulation.GetHeighSimulation() + offset;
        } 
    }

    private void initColonyCamera()
    {
        if(colonyCamera != null)
        {
            colonyCamera.changeToMainCamera();
            int x,z;
            (x,z) = antSimulation.GetAntColonyCoordinate();
            Vector3 coords = new Vector3(x + 20, 15, z);
            colonyCamera.transform.localPosition = coords; 

            GameObject c = GameObject.Find("Colony(Clone)");
            if(c != null)
            {
                colonyCamera.colony = c.GetComponent<Transform>();
                colonyCamera.colonyRadius = (float) AntConlonySimulation.GetGapAroundHill();
            }
        }
    }
    private void initMainCamera()
    {
        if(mainCamera != null)
        {
            float x = antSimulation.GetWidthSimulation() + 1;
            float z = antSimulation.GetHeighSimulation() + 1;
            float y = (Math.Max(x, z) * 2) / 2;
            Vector3 newPos = new Vector3(x / 2 + x / 8, y, z / 2);
            mainCamera.localPosition = newPos;
        }
    }
}
