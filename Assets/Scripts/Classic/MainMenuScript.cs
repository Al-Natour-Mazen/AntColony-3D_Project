using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartSimulation()
    {
        SceneManager.LoadSceneAsync("Simulation");
    }

    public void QuitSimulationApp()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}
