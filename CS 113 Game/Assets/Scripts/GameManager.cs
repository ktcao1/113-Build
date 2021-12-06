using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Resources
    [SerializeField] private List<Sprite> weaponSprites;
    [SerializeField] private Player player;
    [SerializeField] public AudioSource gameMusic;
    [SerializeField] private Image musicSymbol;
    [SerializeField] private Sprite musicOn, musicOff;
    [SerializeField] private RoomTemplates roomTemplates;
    public int roomsCleared = 0;
    public bool disableInputs = false;
    public bool isLoading = true;
    public bool musicPaused = false;
    public bool gamePaused = false;
    public string difficulty;

    // Interactable UI
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject difficultyPanel;
    private Animator pausePanelAnim;

    // UI
    [SerializeField] private TMP_Text scoreText;
    public bool fadingOut, startFading, coinFading;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        pausePanelAnim = pausePanel.GetComponent<Animator>();
        difficultyPanel.SetActive(true);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && !isLoading && !pausePanelAnim.GetBool("show"))
        {
            PauseGame();
        }
    }

    // TODO: Change into a full menu in the future
    public void PauseGame()
    {
        if (disableInputs || player.isDead) return;

        if (pausePanelAnim.GetBool("show"))
        {
            gamePaused = false;
            Time.timeScale = player.currentTimeScale;
            pausePanelAnim.SetBool("show", false);
            if (!musicPaused) gameMusic.UnPause();
        }
        else
        {
            gamePaused = true;
            Time.timeScale = 0;
            pausePanelAnim.SetBool("show", true);
            gameMusic.Pause();
        }
    }

    public void PauseMusic()
    {
        if (player.isDead) return;
        if (!musicPaused)
        {
            musicSymbol.sprite = musicOff;
            gameMusic.Pause();
            musicPaused = true;
        }
        else
        {
            musicSymbol.sprite = musicOn;
            gameMusic.UnPause();
            musicPaused = false;
        }
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneAsync("TitleScene"));
    }

    public void EndScreen()
    {
        gameMusic.Pause();
        gameOverPanel.SetActive(true);
        startFading = true;
    }

    public void VictoryScreen()
    {
        GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("Victory").GetComponent<AudioSource>().Play();
        disableInputs = true;
        coinFading = true;
        gameMusic.Pause();
        victoryPanel.SetActive(true);
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneAsync("SampleScene"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    IEnumerator FadeToFullAlphaImg(float t, Image i, string first)
    {
        while (i.color.a <= 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            if (i.color.a >= 1f && first == "first") fadingOut = true;
            yield return null;
        }
    }

    IEnumerator FadeToFullAlphaText(float t, TMP_Text i)
    {
        while (i.color.a <= 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeToZeroAlphaImg(float t, Image i)
    {
        while (i.color.a >= 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Update()
    {
        scoreText.text = "Rooms Cleared: " + roomsCleared + " / " + roomTemplates.rooms.Count;
    }

    public void FixedUpdate()
    {
        if (startFading)
            StartCoroutine(FadeToFullAlphaImg(50f, gameOverPanel.GetComponent<Image>(), "first"));
        if (fadingOut)
        {
            StartCoroutine(FadeToFullAlphaText(50f, gameOverPanel.transform.Find("DiedText").GetComponent<TMP_Text>()));
            StartCoroutine(FadeToFullAlphaText(50f, gameOverPanel.transform.Find("RestartButton").GetComponentInChildren<TMP_Text>()));
            StartCoroutine(FadeToFullAlphaText(50f, gameOverPanel.transform.Find("BackToTitleButton").GetComponentInChildren<TMP_Text>()));
        }
        if (coinFading)
        {
            StartCoroutine(FadeToZeroAlphaImg(100f, victoryPanel.transform.Find("CoinImage").GetComponent<Image>()));
        }
    }

    // TODO: Change into different UI in the future
    public void IncrementScore()
    {
        int score = Int32.Parse(scoreText.text.Substring(16)) + 1;
        scoreText.text = "Enemies Killed: " + score;
        // if (score == roomsCleared - 1)
        // {
        //     Time.timeScale = 0;
        //     VictoryScreen();
        // }
    }
}
