using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameRunning { get; private set; } = true;


    [Space]
    [Header("Pause Menu Settings")]
    [SerializeField] private CanvasGroup pauseMenuGroup;
    [SerializeField] private float pauseMenuTweenDuration = 0.5f;

    private void Awake ()
    {
        // Singleton Setup
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(this);
            }
        }
        else
        {
            Instance = this;
        }
    }

    private void Update ()
    {
        PauseGame();
    }

    // Used to resume the game. Triggered from the UI resume button.
    public void ResumeGame ()
    {
        TweenPauseMenuUI(false);
        IsGameRunning = true;
    }

    // Used to show and hide UI
    private void TweenPauseMenuUI (bool active)
    {
        if (active == false)
        {
            Time.timeScale = 1f;
        }

        // Kills any tween currently running to avoid confliction, and re-tween depending on the sent active value
        DOTween.Kill(pauseMenuGroup);
        pauseMenuGroup.DOFade(active ? 1f : 0f, pauseMenuTweenDuration)
            .OnComplete(() => { if (active) Time.timeScale = 0f; });
        
        pauseMenuGroup.interactable = active;
        pauseMenuGroup.blocksRaycasts = active;
    }

    // Called in the Update function to pause and unpause the game.
    private void PauseGame ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsGameRunning = !IsGameRunning;

            // If the game is running, show the UI else hide it. So it's always opposite to IsGameRunning
            TweenPauseMenuUI(!IsGameRunning);
        }
    }

    // Used to restart the game. Triggered from the UI restart button.
    public void RestartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    // Used to quit the game. Triggered from the UI quit button.
    public void QuitGame ()
    {
        Application.Quit();
    }
}
