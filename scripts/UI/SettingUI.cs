using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SettingUI : MonoBehaviour
{
    public static SettingUI Instance { get; private set;}

    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button moveUPBtn;
    [SerializeField] private Button moveDownBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;
    [SerializeField] private Button interactBtn;
    [SerializeField] private Button interactAlternateBtn;
    [SerializeField] private Button PauseBtn;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private GameObject presssToRebindKeyObj;

    void Awake()
    {
        soundSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.ChangeVolume(value);
        });

        musicSlider.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance.ChangeVolume(value);
        });

        closeBtn.onClick.AddListener(() =>
        {
            Hide();
            GamePauseUI.Instance.Show();
        });

        moveUPBtn.onClick.AddListener(()=>{RebingBinding(GameInput.Binding.Move_Up);});
        moveDownBtn.onClick.AddListener(()=>{RebingBinding(GameInput.Binding.Move_Down);});
        moveLeftBtn.onClick.AddListener(()=>{RebingBinding(GameInput.Binding.Move_Left);});
        moveRightBtn.onClick.AddListener(()=>{RebingBinding(GameInput.Binding.Move_Right);});
        interactBtn.onClick.AddListener(()=>{RebingBinding(GameInput.Binding.Interact);});
        interactAlternateBtn.onClick.AddListener(()=>{RebingBinding(GameInput.Binding.Interact_Afternate);});
        PauseBtn.onClick.AddListener(()=>{RebingBinding(GameInput.Binding.Pause);});

        Instance = this;
    }

    void Start()
    {
        soundSlider.value = SoundManager.Instance.GetVolume();
        musicSlider.value = MusicManager.Instance.GetVolume();
        
        UpdateVisual();
        Hide();
        HidePresssToRebindKey();
    }

    private void UpdateVisual()
    {
        moveUpText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Interact_Afternate);
        pauseText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void RebingBinding(GameInput.Binding binding)
    {
        ShowPresssToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePresssToRebindKey();
            UpdateVisual();
        });
    }

    private void ShowPresssToRebindKey()
    {
        presssToRebindKeyObj.SetActive(true);
    }

    private void HidePresssToRebindKey()
    {
        presssToRebindKeyObj.SetActive(false);
    }
}
