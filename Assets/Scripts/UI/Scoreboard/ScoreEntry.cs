using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntry : MonoBehaviour {

    private Text placeText;
    private Text timeText;
    private Text scoreText;

    private void Awake()
    {
        placeText = transform.Find("Place").GetComponent<Text>();
        timeText = transform.Find("Time").GetComponent<Text>();
        scoreText = transform.Find("Score").GetComponent<Text>();
    }

    public void SetEntry(DateTime time, int score, int place)
    {
        string placeString = place + ".)";
        placeText.text = placeString;
        timeText.text = time.ToShortDateString() + " " + time.ToShortTimeString();
        scoreText.text = "" + score;
    }
}
