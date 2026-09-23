using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnTrash;

    new public static void ResetStaticData()
    {
        OnTrash = null;
    }

    public override void Interact(Player player)
    {
        base.Interact(player);
        if(player.HasKitchenObject())
        {
            OnTrash?.Invoke(this,EventArgs.Empty);
            player.GetKitchenObject().DestroyKitchenObject();
        }
    }
}
