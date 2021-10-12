using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndUI : MonoBehaviour
{
    public static RoundEndUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform Content;

    public TextMeshProUGUI HeaderMessage;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI ScrapLostText;

    public Button NextLevelButton;

    private bool success;

    public void Activate(bool success, string message, int score, int time, int scrapLost)
    {
        this.success = success;

        NextLevelButton.interactable = success;
        HeaderMessage.text = message;

        ScoreText.text = "" + score;
        TimeText.text = "" + time;
        ScrapLostText.text = "" + scrapLost;

        Content.gameObject.SetActive(true);
    }

    public void OnBack()
    {
        //go to level select
    }

    public void OnRetry()
    {
        GameStateManager.Retry();
        Content.gameObject.SetActive(false);
    }

    public void OnNext()
    {
        if (!success) return;

        //GameStateManager.NextLevel();
    }
}