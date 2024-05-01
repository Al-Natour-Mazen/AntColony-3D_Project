using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmMenu : MonoBehaviour
{
    public string sceneToGo;

    [TextArea(3,10)]
    public string confirmText;
    public string confirmButtonText;
    public string cancelButtonText;

    public Text ConfirmText;
    public Text ConfirmBText;
    public Text CancelBText;

    public bool displayConfirmBox = true;
   

    void Start()
    {
        HideConfirmBox();
        setNewTexts();
    }

    void OnValidate()
    {
        setNewTexts();
    }

    private void setNewTexts()
    {
        if(ConfirmText != null)
        {
            ConfirmText.text = confirmText;
        }
        if(ConfirmBText != null)
        {
            ConfirmBText.text = confirmButtonText;
        }
        if(CancelBText != null)
        {
            CancelBText.text = cancelButtonText;
        }
    }

    public void ChangeScene()
    {
        if(sceneToGo != "" && sceneToGo != null)
        {
            SceneManager.LoadScene(sceneToGo);
        }
    }

    public void DisplayConfirmBox()
    {
        if (displayConfirmBox)
        {
            SetActiveConfirmBox(true);
        }
        else
        {
            ChangeScene();
        }
    }

    public void HideConfirmBox()
    {
        SetActiveConfirmBox(false);
    }

    private void SetActiveConfirmBox(bool active)
    {
        gameObject.SetActive(active);
    }
}

