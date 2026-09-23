using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI gameStartCountdownText;

    private Animator animator;
    private int previousCountdownNumber;

    void Start()
    {
        animator = GetComponent<Animator>();

        KitchenGameManager.Instanse.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender,System.EventArgs e)
    {
        if(KitchenGameManager.Instanse.IsCountdownToStart())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instanse.GetCountdownToStartTimer());
        gameStartCountdownText.text = countdownNumber.ToString();

        if(previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
