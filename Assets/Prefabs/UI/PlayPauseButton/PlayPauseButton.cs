using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButton : MonoBehaviour
{
    public Sprite playImage;
    public Sprite pauseImage;
    private bool isPlaying = false;
    private Image image;
    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();
        image.sprite = playImage; // default to play image
        btn.onClick.AddListener(OnButtonClick); // add event listener
    }

    private void OnButtonClick()
    {
        isPlaying = !isPlaying; 
        if (isPlaying)
        {
            image.sprite = pauseImage;
        }
        else
        {
            image.sprite = playImage;
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}