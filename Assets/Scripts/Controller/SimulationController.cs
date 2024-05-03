using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AntColonyPersistenceManager;
using static AntColonySaverController;

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

    private float lastTime;
    private AntConlonySimulation antSimulation;
    private float simulationSpeed = 1.0f;
    private float elapsedTime = 0f;

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
       
        // Now that we are sure everything is initialized, we can set the max value of the progress bar.
        // We use the total number of seeds outside the colony as the max value.
        progressBar.SetMaxValue(antSimulation.GetTotalSeedOutColony());
        UpdateUI();
    }

    private void UpdateUI()
    {
        simulationSpeed = sliderTextView.GetCurrentValue();
        dynamicTextViewTime.DynamicText = FormatTime(elapsedTime);
        dynamicTextViewAnt.DynamicText = antSimulation.GetAntsInColony().Count.ToString();
        dynamicTextViewSeedInColony.DynamicText = antSimulation.GetTotalSeedInColony().ToString();
        dynamicTextViewSeedOutColony.DynamicText = antSimulation.GetTotalSeedOutColony().ToString();

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

    public void LoadSimulationTEMP()
    { 
        List<antColInfos> antColMinis = AntColonyPersistenceManager.LoadColonyInfo();
        foreach (antColInfos colonyInfo in antColMinis)
        {
            string allColonyInfo = "";
            allColonyInfo += "----- Colony Info " + colonyInfo.number + " -----\n";
            allColonyInfo += "Width: " + colonyInfo.width + "\n";
            allColonyInfo += "Height: " + colonyInfo.height + "\n";
            allColonyInfo += "Ant Colony Coordinate: (" + colonyInfo.X + ", " + colonyInfo.Y + ")\n";
            allColonyInfo += "Number of Ants: " + colonyInfo.NBAnts + "\n";
            allColonyInfo += "Total Seeds in Colony: " + colonyInfo.SeedsInColony + "\n";
            allColonyInfo += "Total Seeds out Colony: " + colonyInfo.SeedsOutColony + "\n";
            allColonyInfo += "Max Seed Quantity on Block: " + colonyInfo.MaxSeedBlock + "\n";
            allColonyInfo += "Gap Around The Hill: " + colonyInfo.GapAroundHill + "\n";
            allColonyInfo += "-----------------------\n";
            Debug.Log(allColonyInfo);
        }
    }

}
