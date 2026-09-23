using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted; 
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOs;
    [SerializeField] private int waitingRecipeMaxCount = 4;
    [SerializeField] private float waitingTime = 2f;

    private int successRepicesCount = 0;

    private Coroutine currentCoroutine;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        waitingRecipeSOs = new List<RecipeSO>();
        StartCreateRepic();
    }

    private void StartCreateRepic()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        currentCoroutine = StartCoroutine(CreateRepice());
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < waitingRecipeSOs.Count; i++)
        {
            RecipeSO recipeSO = waitingRecipeSOs[i];
            if(recipeSO.kitchenObjectSOs.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentMathchesRecipe = true;
                foreach(KitchenObjectSO recipekitchenObjectSO in recipeSO.kitchenObjectSOs)
                {
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if(recipekitchenObjectSO == plateKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound)
                    {
                        plateContentMathchesRecipe = false;
                    }
                }
                if(plateContentMathchesRecipe)
                {
                    Debug.Log("玩家交出了正确的配方");
                    waitingRecipeSOs.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this,EventArgs.Empty);
                    successRepicesCount++;
                    StartCreateRepic();
                    return;
                }
            }
            Debug.Log("玩家没有交出了正确的配方");
            OnRecipeFailed?.Invoke(this,EventArgs.Empty);
        }
    }

    private IEnumerator CreateRepice()
    {
        while(KitchenGameManager.Instanse.IsGamePlaying() && waitingRecipeSOs.Count < waitingRecipeMaxCount)
        {
            yield return new WaitForSeconds(waitingTime);
            RecipeSO recipeSO = recipeListSO.recipeSOs[UnityEngine.Random.Range(0,recipeListSO.recipeSOs.Count)];
            waitingRecipeSOs.Add(recipeSO);
            OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
        }
    }

    void OnDestroy()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    public List<RecipeSO> GetRecipeSOs()
    {
        return waitingRecipeSOs;
    }

    public int GetSuccessRepicesCount()
    {
        return successRepicesCount;
    }
}
