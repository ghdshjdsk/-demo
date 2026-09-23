using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "KitchenObjectSO", order = 0)]
public class KitchenObjectSO : ScriptableObject {
    public Transform prefeb;
    public Sprite sprite;
    public string objectName;
}
