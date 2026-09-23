using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCounter : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObject;

    void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender,Player.OnSelectedCoubterChangeEventArgs e)
    {
        if(e.selectCounter == baseCounter)
        {
            Show();
        }else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach(var obj in visualGameObject)
        {
            obj.SetActive(true);
        }
    }
    private void Hide()
    {
        foreach(var obj in visualGameObject)
        {
            obj.SetActive(false);
        }
    }
}
