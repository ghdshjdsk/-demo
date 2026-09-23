using System;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private int cuttingProgress;

    public event EventHandler OnCut;
    public static event EventHandler OnTryCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    new public static void ResetStaticData()
    {
        OnTryCut = null;
    }

    public override void Interact(Player player)
    {
        if(HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                //玩家手中有物品
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    Debug.Log(GetKitchenObject().GetKitchenObjectSO());
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyKitchenObject();
                    }
                }
            }
            else
            {
                //玩家手中无物品
                GetKitchenObject().SetKitchenObject(player);
            }
        }else
        {
            if(player.HasKitchenObject())
            {
                //玩家手中有物品
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    cuttingProgress = 0;
                    player.GetKitchenObject().SetKitchenObject(this);
                }
            }
            else
            {
                //玩家手中无物品
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        base.InteractAlternate(player);
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());

            OnCut?.Invoke(this,EventArgs.Empty);
            OnTryCut?.Invoke(this,EventArgs.Empty);

            OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
            {
                ProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                GetKitchenObject().DestroyKitchenObject();

                KitchenObject.SpwanKitchenObject(outputKitchenObjectSO,this);
            }
        }
    }

    public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    public KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }else
        {
            return null;
        }
    }

    public CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOs)
        {
            if(cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
