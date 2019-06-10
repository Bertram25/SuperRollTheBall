using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Scoring
{
    public string player;
    public int score;
    public float playTime;
}

public class ScoreMgr : MonoBehaviour
{
    public Text scoreText;
    public Text chronoText;
    public Text youWinText;
    public Text highScoresText;
    private int score = 0;
    private float _playTime;
    private bool _hasFinished = false;
    private List<Scoring> _highscores;

    void Start()
    {
        score = 0;
        _playTime = 0f;
        youWinText.gameObject.SetActive(false);
        highScoresText.gameObject.SetActive(false);
        _hasFinished = false;

        LoadHighScores();
    }

    private void Update()
    {
        UpdatePlayTime();
    }

    private void UpdatePlayTime()
    {
        if (_hasFinished)
        {
            return;
        }

        _playTime += Time.deltaTime;
        chronoText.text = string.Format("Temps : {0}", formatPlayTime(_playTime));
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = $"Score : {score}";
    }

    public void HasFinished()
    {
        youWinText.gameObject.SetActive(true);
        _hasFinished = true;
        
        Scoring scoring = new Scoring();
        scoring.player = "Me";
        scoring.playTime = _playTime;
        scoring.score = score;
        _highscores.Add(scoring);
        SaveHighScores();
        DisplayHighscores();
    }

    private void LoadHighScores()
    {
        _highscores = new List<Scoring>();

        int index = 0;
        bool dataFound = true;
        while (dataFound)
        {
            Scoring scoring = new Scoring();
            scoring.player = PlayerPrefs.GetString("Player_" + index, "");
            scoring.playTime = PlayerPrefs.GetFloat("PlayTime_" + index, -1f);
            scoring.score = PlayerPrefs.GetInt("Score_" + index, -1);
            ++index;
            
            if (scoring.player == "" || Mathf.Approximately(scoring.playTime, -1f) || scoring.score == -1f)
            {
                dataFound = false;
            }
            else
            {
                _highscores.Add(scoring);
            }
        }
        
        _highscores.Sort((first, second) => first.score.CompareTo(second.score));
        _highscores.Reverse();
    }
    
    private void SaveHighScores()
    {
        int index = 0;
        foreach (Scoring scoring in _highscores)
        {
             PlayerPrefs.SetString("Player_" + index, scoring.player);
             PlayerPrefs.SetFloat("PlayTime_" + index, scoring.playTime);
             PlayerPrefs.SetInt("Score_" + index, scoring.score);
             ++index;
        }
        PlayerPrefs.Save();
    }

    private void DisplayHighscores()
    {
        string highscoreString = "<b>Meilleurs scores :</b>\n\n";
        int index = 0;
        foreach (var scoring in _highscores)
        {
            if (index > 10)
            {
                continue;
            }

            highscoreString += scoring.player + " - " + formatPlayTime(scoring.playTime) + " - " + scoring.score + "\n";

            ++index;
        }

        highScoresText.text = highscoreString;
        highScoresText.gameObject.SetActive(true);
    }

    private string formatPlayTime(float playTime)
    {
        int seconds = (int) playTime % 60;
        int minutes = (int) playTime / 60;
        string timeString = "";
        if (minutes > 0)
        {
            timeString = string.Format("{0}:{1:00}s", minutes, seconds);
        }
        else
        {
            timeString = string.Format("{0:00}s", seconds);
        }

        return timeString;
    }
}
