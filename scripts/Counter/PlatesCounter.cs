using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateRemoved;
    public event EventHandler OnPlateSpwaned;

    [SerializeField] private float spwanTime = 3;
    [SerializeField] private int platesCount = 0;
    [SerializeField] private int platesCountMax = 4;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private Coroutine currentCoroutine;

    void Start()
    {
        currentCoroutine = StartCoroutine(SpwanPlates(spwanTime));
    }
    
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(platesCount > 0)
            {
                platesCount--;
                KitchenObject.SpwanKitchenObject(kitchenObjectSO,player);
                OnPlateRemoved?.Invoke(this,EventArgs.Empty);

                if(currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(SpwanPlates(spwanTime));
                }
            }
        }
    }

    private IEnumerator SpwanPlates(float spwanTime)
    {
        while(KitchenGameManager.Instanse.IsGamePlaying() && platesCount < platesCountMax)
        {
            yield return new WaitForSeconds(spwanTime);
            if(platesCount < platesCountMax)
            {
                platesCount++;
                OnPlateSpwaned?.Invoke(this,EventArgs.Empty);
            }
        }

        currentCoroutine = null;
    }

}
