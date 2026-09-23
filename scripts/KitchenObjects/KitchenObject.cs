using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    private IKitchenObject kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObject(IKitchenObject kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if(this.kitchenObjectParent.HasKitchenObject())
        {
            Debug.Log("当前存在物品");
        }
        this.kitchenObjectParent.SetKitchenObject(this);

        transform.SetParent(kitchenObjectParent.GetKitchenObjectFollowTransform());
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObject GetKitchenObject()
    {
        return kitchenObjectParent;
    }

    public void DestroyKitchenObject()
    {
        this.GetKitchenObject().ClearKitchenObject();
        Destroy(gameObject);
    }

    //检测当前物品是否为盘子
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static KitchenObject SpwanKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObject kitchenObject1)
    {
        Transform kit = Instantiate(kitchenObjectSO.prefeb);
        KitchenObject kitchenObject = kit.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObject(kitchenObject1);

        return kitchenObject;
    }
}
