using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    public static GamePauseUI Instance {get; private set;}


    [SerializeField] private Button remuseBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button mainMenuBtn;

    void Awake()
    {
        remuseBtn.onClick.AddListener(() =>
        {
            KitchenGameManager.Instanse.PauseGame();
        });

        settingBtn.onClick.AddListener(() =>
        {
            Hide();
            SettingUI.Instance.Show();
        });

        mainMenuBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        Instance = this;
    }

    void Start()
    {
        KitchenGameManager.Instanse.OnGamePause += KitchenGameManager_OnGamePause;
        KitchenGameManager.Instanse.OnGameUnPause += KitchenGameManager_OnGameUnPause;

        Hide();
    }

    private void KitchenGameManager_OnGamePause(object o,EventArgs e)
    {
        Show();
    }

    private void KitchenGameManager_OnGameUnPause(object o,EventArgs e)
    {
        Hide();
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
