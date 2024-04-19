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

    [Header("Time Settings")]
    public float simulationSpeed = 1.0f;

    [Header("UI Settings")]
    public ProgressBar progressBar;

    private float lastTime;
    private AntConlonySimulation antSimulation;


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
        StartCoroutine(InitializeProgressBar());
    }

    // Coroutine to initialize the progress bar
    IEnumerator InitializeProgressBar()
    {
        // Wait for the end of the frame. This ensures that all other Start and Update methods have been called for this frame.
        // This is useful when we want to make sure that all other objects and their scripts have been initialized before using them.
        yield return new WaitForEndOfFrame();

        // Now that we are sure everything is initialized, we can set the max value of the progress bar.
        // We use the total number of seeds outside the colony as the max value.
        progressBar.SetMaxValue(antSimulation.GetTotalSeedOutColony());
    }
    void Update()
    {
        float currentTime = Time.fixedTime;
        if (simulationSpeed <= (currentTime - lastTime))
        {
            // Mise à jour du modèle
            antSimulation.EvolveTheAntColony();

            Debug.Log("nb fourmis =" + antSimulation.GetAntsInColony().Count);
            Debug.Log("nb graines =" + antSimulation.GetTotalSeedOutColony());
            Debug.Log("nb graines Hill =" + antSimulation.GetTotalSeedInColony());
            Debug.Log("max val ProgressBar =" + progressBar.GetMaxValue());
            Debug.Log("current val ProgressBar =" + progressBar.GetCurrentValue());
           

            // Mise à jour de la vue avec les données du modèle
            progressBar.AdvanceProgress(antSimulation.GetTotalSeedInColony());
            board.GenerateAnts(antSimulation);
            board.GenerateSeeds(antSimulation);

            lastTime = currentTime; // Met à jour le temps de la dernière mise à jour
        }
    }
}
