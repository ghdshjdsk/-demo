using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    private List<KitchenObjectSO> kitchenObjectsList;
    [SerializeField] private List<KitchenObjectSO> vaidkitchenObjectsList;

    void Awake()
    {
        kitchenObjectsList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!vaidkitchenObjectsList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if(kitchenObjectsList.Contains(kitchenObjectSO))
        {
            return false;
        }else
        {
            kitchenObjectsList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this,new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectsList;
    }
}
