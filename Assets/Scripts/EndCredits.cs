using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndCredits : MonoBehaviour
{
    public Texture2D cursor;
    private AudioManager _audio;
    [SerializeField] private TMP_Text collectedCherriesText;
    [SerializeField] private TMP_Text deathsText;
    [SerializeField] private TMP_Text cherryGrade;
    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.gameCompleted = true;
        PlayerStats.CalculateTotal();
        _audio = FindObjectOfType<AudioManager>();
        collectedCherriesText.text += $"{PlayerStats.collectedCherries} / {PlayerStats.totalCherries}";
        deathsText.text += PlayerStats.deaths;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);

        cherryGrade.SetText(PlayerStats.collectedCherries < PlayerStats.totalCherries
            ? "Try to find all the cherries!"
            : "You've found all the cherries! Congratulations!");
    }

    private void Update()
    {
        if (!_audio.musicPlayed) return;
        if (PlayerStats.collectedCherries < PlayerStats.totalCherries)
        {
            BackToMenu();
        }
        else
        {
            SceneManager.LoadScene(5);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    
}
