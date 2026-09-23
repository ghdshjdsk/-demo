using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter,IKitchenObject
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnplayGrabbedObject;

    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            
        }else
        {
            KitchenObject.SpwanKitchenObject(kitchenObjectSO,player);
            OnplayGrabbedObject?.Invoke(this,EventArgs.Empty);
        }
    }
}
