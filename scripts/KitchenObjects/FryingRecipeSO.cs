using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FryingRecipeSO", menuName = "FryingRecipeSO", order = 0)]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;

    public int FryingProgressMax;
    public State state;
}
