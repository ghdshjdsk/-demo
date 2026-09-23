using UnityEngine;

public class DeliverManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    void Start()
    {
        if(DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
            DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
            UpdateVisual();
        }
    }

    private void DeliveryManager_OnRecipeSpawned(object sender,System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender,System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in container)
        {
            if(child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        if(DeliveryManager.Instance.GetRecipeSOs() != null)
        {
            foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetRecipeSOs())
            {
                Transform recipeTransform = Instantiate(recipeTemplate,container);
                recipeTransform.gameObject.SetActive(true);
                recipeTransform.GetComponent<RecipeTemplateSingleUI>().SetRecipeSO(recipeSO);
            }
        }
    }

}
