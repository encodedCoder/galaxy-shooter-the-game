using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private int currScore = 0, bestScore = 0;
    [SerializeField]
    private Text currScoreText;
    [SerializeField]
    private Text bestScoreText;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text restartGameText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image livesDisplayImage;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        currScore = 0;
        //*********************************************************************
            //Use this to Reset the score to zero before builing the game
            //PlayerPrefs.SetInt("bestScore", 0);
        //*********************************************************************
        bestScore = PlayerPrefs.GetInt("bestScore", 0);
        bestScoreText.text = "Best: " + bestScore;
        currScoreText.text = "Score: " + currScore;
        gameOverText.text = "";
        restartGameText.gameObject.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("GameManger is null");
        }
    }

    public void UpdateScore(int playerScore)
    {
        currScore = playerScore;
        currScoreText.text = "Score: " + currScore;
    }

    public void UpdateLivesImage(int currLives = 3)
    {
        currLives = currLives <= 0 ? 0 : currLives;
        livesDisplayImage.sprite = _livesSprites[currLives];
        if(currLives < 1)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        if (bestScore < currScore)
        {
            //Debug.Log(bestScore + " " + currScore);
            bestScore = currScore;
            bestScoreText.text = "Best: " + bestScore;
            PlayerPrefs.SetInt("bestScore", bestScore);
        }
        gameManager.GameOver();
        restartGameText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlickerRoutine());
    }

    IEnumerator GameOverTextFlickerRoutine(bool toggleState = true)
    {
        while (true)
        {
            gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
