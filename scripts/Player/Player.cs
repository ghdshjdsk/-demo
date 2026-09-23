using System;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObject
{
    public static Player Instance{get; private set;}

    [SerializeField] private float speed = 7;
    [SerializeField] private float rotateSpeed = 10;
    [SerializeField] private bool isWalking;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;

    private Vector2 inputDiraction;
    private bool canMove;

    private float playerRadius = 0.7f;
    private float playerHeight = 0.7f;

    private BaseCounter selectedCounter;

    //HandleInteraction
    private float interations = 2f;

    public event EventHandler OnObjectUp;

    public event EventHandler<OnSelectedCoubterChangeEventArgs> OnSelectedCounterChange;
    public class OnSelectedCoubterChangeEventArgs : EventArgs
    {
        public BaseCounter selectCounter;
    }

    void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Errow");
        }
        Instance = this;
    }

    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAfternateAction += GameInput_OnInteractAlternateAction;
    }

    void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void GameInput_OnInteractAction(object sender,System.EventArgs e)
    {
        if(!KitchenGameManager.Instanse.IsGamePlaying()) return;
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender,System.EventArgs e)
    {
        if(!KitchenGameManager.Instanse.IsGamePlaying()) return;
        if(selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void HandleInteractions()
    {
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit raycastHit,interations,counterLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }else
            {
                SetSelectedCounter(null);
            }
        }else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        inputDiraction = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputDiraction.x,0,inputDiraction.y);

        isWalking = inputDiraction.magnitude > 0.01f;

        canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir,speed * Time.deltaTime);

        if(inputDiraction.magnitude > 0.01f)
        {
            if(!canMove)
            {
                //当物体不能移动且玩家X方向上有移动
                Vector3 moveDirX = new Vector3(moveDir.x,0,0);
                canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX,speed * Time.deltaTime);
                if(canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    //当物体不能移动且玩家Z方向上有移动
                    Vector3 moveDirZ = new Vector3(0,0,moveDir.z);
                    canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ,speed * Time.deltaTime);
                    if(canMove)
                    {
                        moveDir = moveDirZ;
                    }else
                    {
                        //当物体不能移动
                    }
                }
                
            }
            if(canMove)
            {
                transform.position += moveDir * speed * Time.deltaTime;
            }

        }
        transform.forward = Vector3.Slerp(transform.forward,moveDir,rotateSpeed * Time.deltaTime);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChange?.Invoke(this,new OnSelectedCoubterChangeEventArgs
        {
            selectCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject == null)
        {
            OnObjectUp?.Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
