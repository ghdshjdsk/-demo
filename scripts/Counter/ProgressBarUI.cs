using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasHasProgressGameObject;

    private IHasProgress hasProgress;

    void Awake()
    {
        hasProgress = hasHasProgressGameObject.GetComponent<IHasProgress>();
    }

    void Start()
    {
        if(hasProgress == null)
        {
            Debug.Log("空引用异常");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender,IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.ProgressNormalized;

        if(e.ProgressNormalized == 0 || e.ProgressNormalized == 1)
        {
            Hide();
        }else
        {
            Show();
        }
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
