using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI successRepicesCount;

    void Start()
    {
        KitchenGameManager.Instanse.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender,System.EventArgs e)
    {
        if(KitchenGameManager.Instanse.IsGameOver())
        {
            Show();
            successRepicesCount.text = DeliveryManager.Instance.GetSuccessRepicesCount().ToString();
        }
        else
        {
            Hide();
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
