using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] Text highscore;
    [SerializeField] Text score;
    [SerializeField] Text coins;
    [SerializeField] Button reset;
    [SerializeField] Button exit;
    void Awake()
    {
        HighscoreHandler.UpdateScore += UpdateScore;
        HighscoreHandler.UpdateHighscore += UpdateHighscore;
        HighscoreHandler.UpdateCoins += UpdateCoins;
        CollisionHandler.GameEnd += EnableMenu;
        reset.onClick.AddListener(LoadScene);
        exit.onClick.AddListener(Exit);
    }

    private void OnDestroy()
    {
        HighscoreHandler.UpdateScore -= UpdateScore;
        HighscoreHandler.UpdateHighscore -= UpdateHighscore;
        HighscoreHandler.UpdateCoins -= UpdateCoins;
        CollisionHandler.GameEnd -= EnableMenu;
    }

    private void UpdateScore(int score)
    {
        this.score.text = score.ToString();
    }

    private void UpdateHighscore(int highscore)
    {
        this.highscore.text = highscore.ToString();
    }

    private void UpdateCoins(int coins)
    {
        this.coins.text = coins.ToString();
    }

    private void EnableMenu()
    {
        reset.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);

    }

    private void LoadScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    private void Exit()
    {
        Application.Quit();
    }
}
