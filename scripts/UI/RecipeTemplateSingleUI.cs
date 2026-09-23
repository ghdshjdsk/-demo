using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTemplateSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;

    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeName.text = recipeSO.recipeName;
        foreach(Transform child in iconContainer)
        {
            if(child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOs)
        {
            Transform icon = Instantiate(iconTemplate,iconContainer);
            icon.gameObject.SetActive(true);
            icon.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
