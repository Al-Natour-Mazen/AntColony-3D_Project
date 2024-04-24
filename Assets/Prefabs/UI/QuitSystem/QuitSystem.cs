using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitSystem : MonoBehaviour
{
    public GameObject quitBox;
    public string sceneToGo;

    void Start()
    {
        HideQuitBox();

    }

    public void DisplayQuitBox()
    {
        SetActiveQuitBox(true);
    }

    public void HideQuitBox()
    {
        SetActiveQuitBox(false);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToGo);
    }

    private void SetActiveQuitBox(bool active)
    {
        if(quitBox != null)
        {
            quitBox.SetActive(active);
        }
    }
}
