using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    public static KitchenGameManager Instanse {get; private set;}

    [SerializeField] private State state;
    [SerializeField] private float countdownToStartTimer = 3f;
    private float gamePlayingTimer = 0;
    [SerializeField] private float gamePlayingTimerMax = 100f;

    private bool gameIsPause;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameUnPause;

    void Awake()
    {
        Instanse = this;
        state = State.WaitingToStart;
    }

    void Start()
    {
        GameInput.Instance.OnPauseAction += Pause;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender,System.EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this,EventArgs.Empty);
        }
    }

    private void Pause(object sender,EventArgs e)
    {
        PauseGame();
    }

    void Update()
    {
        switch(state)
        {
            case State.WaitingToStart:
            break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if(countdownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
            break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer <= 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
            break;
            case State.GameOver:
            break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStart()
    {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimer()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);  
    }

    public void PauseGame()
    {
        gameIsPause = !gameIsPause;
        if(gameIsPause)
        {
            Time.timeScale = 0;

            OnGamePause?.Invoke(this,EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;

            OnGameUnPause?.Invoke(this,EventArgs.Empty);
        }
    }
}
