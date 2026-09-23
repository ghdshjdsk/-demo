using System;
using System.Collections;
using UnityEngine;

public enum State
{
    Idle,
    Frying,
    Fried,
    Burned
}

public class StoveCounter : BaseCounter,IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;

    private float progress;

    private Coroutine currentFryingCoroutine;

    private FryingRecipeSO fryingRecipeSO;
    [SerializeField] private State currentState;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    void Start()
    {
        currentState = State.Idle;
    }

    void Update()
    {
        
    }

    public override void Interact(Player player)
    {
        if(HasKitchenObject())//有物品
        {
            if(player.HasKitchenObject())
            {
                //玩家手中有物品
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyKitchenObject();

                        currentState = State.Idle;
                        StopCurrentFryingCoroutine();

                        OnStateChanged?.Invoke(this,new OnStateChangedEventArgs{ state = currentState });
                    }
                }
            }
            else
            {
                //玩家手中无物品
                GetKitchenObject().SetKitchenObject(player);
                currentState = State.Idle;
                StopCurrentFryingCoroutine();

                OnStateChanged?.Invoke(this,new OnStateChangedEventArgs{ state = currentState });
            }
        }else
        {
            if(player.HasKitchenObject())
            {
                //玩家手中有物品
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObject(this);
                    fryingRecipeSO = GetFryingRecipeSO(GetKitchenObject().GetKitchenObjectSO());

                    currentState = fryingRecipeSO.state;

                    fryingRecipeSO = GetCurrentFryingRecipeSO(currentState);

                    currentFryingCoroutine = StartCoroutine(FryingKitchenObject());

                    OnStateChanged?.Invoke(this,new OnStateChangedEventArgs{ state = currentState });
                }
            }
            else
            {
                //玩家手中无物品
            }
        }
    }

    private void StopCurrentFryingCoroutine()
    {
        if (currentFryingCoroutine != null)
        {
            StopCoroutine(currentFryingCoroutine);
            currentFryingCoroutine = null;
        }
        
        // 重置所有状态
        currentState = State.Idle;
        progress = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            ProgressNormalized = 0f
        });
    }

    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {

        }
    }

    public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    public KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(inputKitchenObjectSO);
        if(fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }else
        {
            return null;
        }
    }

    public FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOs)
        {
            if(fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    public FryingRecipeSO GetCurrentFryingRecipeSO(State state)
    {
        switch(state)
        {
            case State.Frying:
                return fryingRecipeSOs[0];
            case State.Fried:
                return fryingRecipeSOs[1];
        }
        return null;
    }

    private IEnumerator FryingKitchenObject()
    {
        while(currentState != State.Burned)
        {
            switch(currentState)
            {
                case State.Idle:
                    
                    break;
                case State.Frying:
                    progress = 0;
                    while(progress < fryingRecipeSO.FryingProgressMax)
                    {
                        progress += Time.deltaTime;
                        OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = progress / fryingRecipeSO.FryingProgressMax
                        });
                        yield return null;
                    }
                    GetKitchenObject().DestroyKitchenObject();

                    KitchenObject.SpwanKitchenObject(fryingRecipeSO.output,this);
                    currentState = State.Fried;

                    OnStateChanged?.Invoke(this,new OnStateChangedEventArgs{ state = currentState });
                    break;
                case State.Fried:
                    progress = 0;
                    while(progress < GetCurrentFryingRecipeSO(currentState).FryingProgressMax)
                    {
                        progress += Time.deltaTime;
                        OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = progress / GetCurrentFryingRecipeSO(currentState).FryingProgressMax
                        });
                        yield return null;
                    }
                    GetKitchenObject().DestroyKitchenObject();

                    KitchenObject.SpwanKitchenObject(GetCurrentFryingRecipeSO(currentState).output,this); 
                    currentState = State.Burned;

                    OnStateChanged?.Invoke(this,new OnStateChangedEventArgs{ state = currentState });
                    break;
                case State.Burned:
                    progress = 0;
                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                            {
                                ProgressNormalized = 0
                            });
                    break;
            }
        }
    }
}
