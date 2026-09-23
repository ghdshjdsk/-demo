using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image gamePlayingClock;

    void Update()
    {
        gamePlayingClock.fillAmount = KitchenGameManager.Instanse.GetGamePlayingTimer();
    }
}
