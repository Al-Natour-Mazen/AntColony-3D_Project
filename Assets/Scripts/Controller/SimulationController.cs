using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{

    [Header("Board Settings")]
    public Board board;
    public int width = 20;
    public int height = 10;

    [Header("Entity Counts")]
    public int antCount = 10;
    public int wallCountDensity = 10;
    public int seedCountDensity = 10;

    [Header("Seed Settings")]
    public int maxSeedQuantityPerBlock = 4;

   

    [Header("UI Settings")]
    public ProgressBar progressBar;
    public PlayPauseButton playPauseButton;
    public SliderTextView sliderTextView;
    public StaticDynamicTextView dynamicTextViewAnt;
    public StaticDynamicTextView dynamicTextViewTime;
    public StaticDynamicTextView dynamicTextViewSeedInColony;
    public StaticDynamicTextView dynamicTextViewSeedOutColony;
    public Menu menu;

    private float lastTime;
    private AntConlonySimulation antSimulation;
    private float simulationSpeed = 1.0f;
    private float elapsedTime = 0f;


    void Start()
    {
        // Initialisation du modèle avec les données
        antSimulation = new AntConlonySimulation(width, height, maxSeedQuantityPerBlock, wallCountDensity, antCount, seedCountDensity);

        // Initialisation de la vue avec les données du modèle
        board.SetHeightBoard(height);
        board.SetWidthBoard(width);
        board.InstantiateAnthill(antSimulation);
        board.GenerateWalls(antSimulation);
        board.GenerateAnts(antSimulation);
        board.GenerateSeeds(antSimulation);

        // Start the coroutine to initialize the progress bar
        StartCoroutine(InitializeUIComponents());

        
    }

    // Coroutine to initialize the progress bar
    IEnumerator InitializeUIComponents()
    {
        // Wait for the end of the frame. This ensures that all other Start and Update methods have been called for this frame.
        // This is useful when we want to make sure that all other objects and their scripts have been initialized before using them.
        yield return new WaitForEndOfFrame();

        // Now that we are sure everything is initialized, we can set the max value of the progress bar.
        // We use the total number of seeds outside the colony as the max value.
        progressBar.SetMaxValue(antSimulation.GetTotalSeedOutColony());
        simulationSpeed = sliderTextView.GetCurrentValue();
        dynamicTextViewTime.DynamicText = FormatTime(elapsedTime);
        dynamicTextViewAnt.DynamicText = antSimulation.GetAntsInColony().Count.ToString();
        dynamicTextViewSeedInColony.DynamicText = antSimulation.GetTotalSeedInColony().ToString();
        dynamicTextViewSeedOutColony.DynamicText = antSimulation.GetTotalSeedOutColony().ToString();
    }
    void Update()
    {
        float currentTime = Time.fixedTime;
        float sliderValue = sliderTextView.GetCurrentValue();

        if (sliderValue <= 0)
        {
            simulationSpeed = 1.0f;
        }
        else
        {
            // Inverse the slider value before assigning it to simulationSpeed
            simulationSpeed = (1.2f / (sliderValue + 0.001f * 100));
        }

        if (playPauseButton.IsPlaying())
        {
            if (simulationSpeed <= (currentTime - lastTime))
            {
                menu.DisableButton();

                // Mise à jour du modèle
                antSimulation.EvolveTheAntColony();

                dynamicTextViewSeedInColony.DynamicText = antSimulation.GetTotalSeedInColony().ToString();
                dynamicTextViewSeedOutColony.DynamicText = antSimulation.GetTotalSeedOutColony().ToString();

                // Mise à jour de la vue avec les données du modèle
                progressBar.AdvanceProgress(antSimulation.GetTotalSeedInColony());
                board.GenerateAnts(antSimulation);
                board.GenerateSeeds(antSimulation);

                lastTime = currentTime; // Met à jour le temps de la dernière mise à jour
            }

            elapsedTime += Time.deltaTime * sliderValue; // Always increase elapsedTime when the simulation is playing
            dynamicTextViewTime.DynamicText = FormatTime(elapsedTime);
        }
        else
        {
            menu.EnableButton();
        }
    }

    // Méthode pour formater le temps en format lisible (heures:minutes:secondes)
    string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
