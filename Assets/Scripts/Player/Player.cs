using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Experimental.AI;

public class Player : MonoBehaviour, IHasPersistentData
{
    public bool IsWalking {get; private set; }
    public event EventHandler<OnSceneLoaderCollidedEventArgs> OnSceneLoaderCollided;
    public class OnSceneLoaderCollidedEventArgs : EventArgs
    {
        public GameObject sceneLoaderGameObject;
    }
    public event EventHandler<OnPickupEventArgs> OnPickup;
    public class OnPickupEventArgs : EventArgs
    {
        public GameItemSO gameItemSO;
        public GameObject gameItemGameObject;
    }

    public bool IsGrounded {get; private set; }
    private Rigidbody rb;
    private Vector3 lastMoveDir;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float jumpHeight = 15f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    public bool DataSuccessfullyWritten { get; private set; }
    public static Player Instance { get; private set; } //PLAYER SINGLETON
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        //some scenes shouldnt have the player in them so just prefab the player in every necessary scene
    }
    private void Start()
    {
        IsGrounded = true;
        rb = GetComponent<Rigidbody>();
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        LoadGameData();
    //     gameInput.OnInteractAction += GameInput_OnInteractAction;
    //     gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    private void DayManager_OnDayEnd(object sender, EventArgs e)
    {
        WriteToGameData();
    }
    private void OnSceneTransitionHandler(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }
    private void OnDestroy()
    {
        gameInput.OnJumpAction -= GameInput_OnJumpAction;
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
    }
    private void GameInput_OnJumpAction(object sender, System.EventArgs e)
    {
        if (IsGrounded)
        {
            IsGrounded = false;
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        }
    }

    void Update()
    {
        HandleMovement();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            StartCoroutine(getOffNPC());
        }
    }
    private IEnumerator getOffNPC ()
    {
        // Wait for a short duration to allow the player to move away from the NPC
        yield return new WaitForSeconds(0.1f);
        // Check if the player is still colliding with the NPC
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("NPC"))
            {
                // If still colliding, move the player slightly away from the NPC
                Vector3 directionAwayFromNPC = (transform.position - collider.transform.position).normalized;
                transform.position += directionAwayFromNPC * 0.1f; // Move the player away by 0.1 units
            }
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        //IN THE UFTURE ITEMS WONT BE PICKED UP UPON TOUCH
        //THE PLAYER WILL COLLIDE AND HIT A BUTTON TO PICK UP THE ITEM
        if (other.gameObject.CompareTag("Item"))
        {
            GameItem gameItem = other.GetComponent<GameItem>();
            OnPickup?.Invoke(this, new OnPickupEventArgs
            {
                gameItemSO = gameItem.GetGameItemSO(),
                gameItemGameObject = gameItem.gameObject
            });
        }    
        if (other.gameObject.CompareTag("SceneLoader"))
        {
            OnSceneLoaderCollided?.Invoke(this, new OnSceneLoaderCollidedEventArgs
            {
                sceneLoaderGameObject = other.gameObject
            });
        }
    }
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

        if (moveDir.x != 0)
        {
            lastMoveDir = moveDir;
        }

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .6f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(
            transform.position + Vector3.up * 0.1f,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore
        );



        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirX,
                moveDistance,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore
            );
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance,
                    Physics.AllLayers,
                    QueryTriggerInteraction.Ignore
                );
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
            IsWalking = moveDir != Vector3.zero;
        }


        if (lastMoveDir.x < 0)
        {
            transform.forward = Vector3.Slerp(transform.forward, new Vector3(0, 0, 1), Time.deltaTime * rotateSpeed); //turning player to face the direction of movement
        }
        else if (lastMoveDir.x > 0)
        {
            transform.forward = Vector3.Slerp(transform.forward, new Vector3(0, 0, -1), Time.deltaTime * rotateSpeed); //turning player to face the direction of movement
        }
    
    }
    public void WriteToGameData()
    {
        GameData.Instance.PlayerFacingDirection = transform.forward;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData && GameData.Instance.HasPendingSpawnPosition)
        {
            transform.localPosition = GameData.Instance.PlayerSpawnPosition; //LOCAL
            transform.forward = GameData.Instance.PlayerFacingDirection;
            GameData.Instance.HasPendingSpawnPosition = false;
        }
    }
}
