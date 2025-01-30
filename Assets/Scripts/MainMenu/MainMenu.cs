using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioSource backgroundAudioSource;
    [SerializeField]
    private GameObject quiteButton;
    [SerializeField]
    private GameObject quiteConfirm;
    private bool gamePaused = false;

    void Start()
    {
        gamePaused = false;
        quiteButton = GameObject.Find("Quite");
        quiteButton.SetActive(true);
        quiteConfirm = GameObject.Find("QuiteConfirm");
        quiteConfirm.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    public void LoadGame()
    {
        SceneManager.LoadScene(1); // Game Scene
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        backgroundAudioSource.Pause();
        quiteButton.SetActive(false);
        quiteConfirm.SetActive(true);
    }

    public void ResumGame()
    {
        quiteButton.SetActive(true);
        Time.timeScale = 1f;
        gamePaused = false;
        backgroundAudioSource.Play();
        quiteConfirm.SetActive(false);
    }
    public void QuiteGame()
    {
        Application.Quit();
    }
}
