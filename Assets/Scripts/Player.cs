using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    

    private bool isWalking;
    //private Vector3 lastInteractDir;
    //private BaseCounter selectedCounter;
    //private KitchenObject kitchenObject;
    
    // [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    // [SerializeField] private LayerMask countersLayerMask;

    // public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    // public class OnSelectedCounterChangedEventArgs : EventArgs
    // {
    //     public BaseCounter selectedCounter;
    // }


    public static Player Instance { get; private set; } //PLAYER SINGLETON
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    private void Start()
    {
    //     gameInput.OnInteractAction += GameInput_OnInteractAction;
    //     gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    // private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    // {
    //     if (selectedCounter != null)
    //     {
    //         selectedCounter.Interact(this);
    //     }
    // }
    // private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    // {
    //     if (selectedCounter != null)
    //     {
    //         selectedCounter.InteractAlternate(this);
    //     }
    // }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        //HandleInteractions();
    }

    // public bool IsWalking()
    // {
    //     return isWalking;
    // }

    // private void HandleInteractions()
    // {
    //     Vector2 inputVector = gameInput.GetMovementVectorNormalized();
    //     Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

    //     if (moveDir != Vector3.zero)
    //     {
    //         lastInteractDir = moveDir;
    //     }

    //     float interactDistance = 2f;
    //     if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
    //     {
    //         if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
    //         {
    //             //clearCounter.Interact();
    //             if (baseCounter != selectedCounter)
    //             {
    //                 SetSelectedCounter(baseCounter);
    //             }
                
    //         }
    //         else
    //         {
    //             SetSelectedCounter(null);
    //         }
    //     }
    //     else
    //     {
    //         SetSelectedCounter(null);
    //     }
    //     //Debug.Log(selectedCounter);
    // }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;


        // float playerRadius = .6f;
        // float playerHeight = 2f;
        //bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // if (!canMove)
        // {
        //     Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
        //     canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
        //     if (canMove)
        //     {
        //         moveDir = moveDirX;
        //     }
        //     else
        //     {
        //         Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
        //         canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
        //         if (canMove)
        //         {
        //             moveDir = moveDirZ;
        //         }
        //     }
        // }
        // if (canMove)
        // {
        transform.position += moveDir.normalized * moveDistance;
        // }

        // isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 15f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); //turning player to face the direction of movement
    }

    // private void SetSelectedCounter(BaseCounter selectedCounter)
    // {
    //     this.selectedCounter = selectedCounter;

    //     OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
    //     {
    //         selectedCounter = selectedCounter
    //     });
    // }
}
