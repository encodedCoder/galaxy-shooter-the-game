using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver = false;
    private bool gamePaused = false;
    //[SerializeField]
    //private AudioClip backgroundAudioClip;
    [SerializeField]
    private AudioSource backgroundAudioSource;
    [SerializeField]
    private GameObject quiteButton;
    [SerializeField]
    private GameObject quiteConfirm;
    private GameObject gameOverBGPanel;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gamePaused = false;
        gameOverBGPanel = GameObject.Find("GameOverBGPanel");
        if(gameOverBGPanel != null)
        {
            gameOverBGPanel.SetActive(false);
        }
        else
        {
            Debug.Log("gameOverBGPanel GameObject is not present in the scene");
        }

        try
        {
            backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
            if(backgroundAudioSource == null)
            {
                print("BackgroundMusic failed to load from previous scene");
            }
            else
            {
                print("BackgroundMusic loaded from previous scene");
            }
        }
        catch
        {
            backgroundAudioSource = GameObject.Find("BackgroundMusic_Backup").GetComponent<AudioSource>();
            print("Background music is loaded from current scene");
        }
        backgroundAudioSource.Play();
        quiteButton = GameObject.Find("Quite");
        quiteConfirm = GameObject.Find("QuiteConfirm");
        quiteButton.SetActive(true);
        quiteConfirm.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); // Current Game Scene
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverBGPanel.SetActive(true);
        backgroundAudioSource.Stop();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        try
        {
            backgroundAudioSource.Pause();
        }
        catch
        {
            Debug.Log("Cannot pause: BackgroundMusic failed to load from previous scene");
        }
        quiteButton.SetActive(false);
        quiteConfirm.SetActive(true);
    }

    public void QuiteGame()
    {
        Application.Quit();
    }

    public void ResumGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        if (!isGameOver && backgroundAudioSource != null)
        {
            backgroundAudioSource.Play();
        }
        quiteButton.SetActive(true);
        quiteConfirm.SetActive(false);
    }
}
