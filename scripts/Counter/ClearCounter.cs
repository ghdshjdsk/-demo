using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter,IKitchenObject
{
    public override void Interact(Player player)
    {
        if(HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                //玩家手中有物品
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyKitchenObject();
                    }
                }else
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroyKitchenObject();
                        }
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
                player.GetKitchenObject().SetKitchenObject(this);
            }
            else
            {
                //玩家手中无物品
            }
        }
    }
}
