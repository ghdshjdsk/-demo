using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LearningUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpKeyText;
    [SerializeField] private TextMeshProUGUI moveDownKeyText;
    [SerializeField] private TextMeshProUGUI moveLeftKeyText;
    [SerializeField] private TextMeshProUGUI moveRightKeyText;
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private TextMeshProUGUI altKeyText;
    [SerializeField] private TextMeshProUGUI pauseKeyText;

    void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instanse.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
    }

    private void GameInput_OnBindingRebind(object sender,System.EventArgs e)
    {
        UpdateVisual();
    }

    private void KitchenGameManager_OnStateChanged(object sender,System.EventArgs e)
    {
        if(KitchenGameManager.Instanse.IsCountdownToStart())
        {
            Hide();
        }
    }


    private void UpdateVisual()
    {
        moveUpKeyText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Up);
        moveDownKeyText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Down);
        moveLeftKeyText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Left);
        moveRightKeyText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Right);
        interactKeyText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Interact);
        altKeyText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Interact_Afternate);
        pauseKeyText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Pause);
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

