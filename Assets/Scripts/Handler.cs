using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Handler : MonoBehaviour
{
    public TileBoard board;
    public CanvasGroup gameOver;
    public CanvasGroup controlButtons;
    public CanvasGroup menu;
    public CanvasGroup scoreboard;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public string difficulty;
    public int score { get; private set; } = 0;

    private void Start()
    {
        Menu();
    }

    public void Menu()
    {
        gameOver.alpha = 0;
        gameOver.interactable = false;
        controlButtons.alpha = 0;
        controlButtons.interactable = false;
        scoreboard.alpha = 0;
        menu.alpha = 1;
        menu.interactable = true;
        difficulty = null;
    }

    public void StartEasy()
    {
        difficulty = "Easy";
        NewGame();
        
    }
    
    public void StartMedium()
    {
        difficulty = "Medium";
        NewGame();
    }

    public void StartHard()
    {
        difficulty = "Hard";
        NewGame();
    }
    public void NewGame()
    {
        SetScore(0);
        highScoreText.text = LoadHighScore(difficulty).ToString();
        gameOver.alpha = 0;
        gameOver.interactable = false;
        menu.alpha = 0;
        menu.interactable = false;
        board.ClearGame();
        board.SpawnStart();
        scoreboard.alpha = 1;
        controlButtons.alpha = 1;
        controlButtons.interactable = true;
        board.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.alpha = 1;
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        SetHighScore();
    }

    private void SetHighScore()
    {
        int highscore = LoadHighScore(difficulty);
        if (score > highscore)
        {
            PlayerPrefs.SetInt(difficulty, score);
        }
    }

    private int LoadHighScore(string difficulty)
    {
        return PlayerPrefs.GetInt(difficulty, 0);
    }

    public void AddScore(int points)
    {
        SetScore(score + points);
    }
}
