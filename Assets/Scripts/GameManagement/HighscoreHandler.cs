using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreHandler : MonoBehaviour
{
    public delegate void Highscore(int score);
    public static event Highscore UpdateHighscore;
    public static event Highscore UpdateScore;
    public static event Highscore UpdateCoins;

    [SerializeField] private Transform player;

    private int highestScore;
    private int currentScore;
    private int currentCoins;

    private void Awake()
    {
        CollisionHandler.GameEnd += Death;
        CollisionHandler.CoinPickup += CoinsHandler;
        highestScore = PlayerPrefs.GetInt("Highscore", 0);
        currentCoins = PlayerPrefs.GetInt("Coins", 0);
    }

    private void Start()
    {
        UpdateHighscore(highestScore);
        UpdateCoins(currentCoins);
        UpdateScore(0);
    }

    private void FixedUpdate()
    {
        int playerY = (int)player.position.y;
        if (playerY > currentScore)
        {
            currentScore = playerY;
            UpdateScore(currentScore);
            if(currentScore> highestScore)
            {
                highestScore = currentScore;
                UpdateHighscore(highestScore);
            }
        }
    }
    private void CoinsHandler()
    {
        currentCoins++;
        UpdateCoins(currentCoins);
    }

    private void OnDestroy()
    {
        CollisionHandler.GameEnd -= Death;
        CollisionHandler.CoinPickup -= CoinsHandler;
    }

    private void Death()
    {
        if(currentScore == highestScore)
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
        }
        PlayerPrefs.SetInt("Coins", currentCoins);
    }

}
