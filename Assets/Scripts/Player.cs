using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Experimental.AI;

public class Player : MonoBehaviour, IHasPersistentData
{
    public event EventHandler OnSceneLoaderCollided;
    public event EventHandler<OnPickupEventArgs> OnPickup;
    public class OnPickupEventArgs : EventArgs
    {
        public GameItemSO gameItemSO;
        public GameObject gameItemGameObject;
    }

    private bool isGrounded = true;
    private Rigidbody rb;
    private Vector3 lastMoveDir;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float jumpHeight = 15f;
    [SerializeField] private GameInput gameInput;
    // [SerializeField] private LayerMask countersLayerMask;

    // public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    // public class OnSelectedCounterChangedEventArgs : EventArgs
    // {
    //     public BaseCounter selectedCounter;
    // }
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
        rb = GetComponent<Rigidbody>();
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        LoadGameData();
    //     gameInput.OnInteractAction += GameInput_OnInteractAction;
    //     gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
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
        Debug.Log("Jumping");
        if (isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        }
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
        //Debug.Log(isGrounded);
        HandleMovement();
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("SceneLoader"))
        {
            OnSceneLoaderCollided?.Invoke(this, EventArgs.Empty);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        //IN THE UFTURE ITEMS WONT BE PICKED UP UPON TOUCH
        //THE PLAYER WILL COLLIDE AND HIT A BUTTON TO PICK UP THE ITEM
        if (other.gameObject.CompareTag("Item"))
        {
            //Debug.Log("I AM THE PLAYER AND I HAVE COLLIDED WITH AN ITEM");
  
            GameItem gameItem = other.GetComponent<GameItem>();
            OnPickup?.Invoke(this, new OnPickupEventArgs
            {
                gameItemSO = gameItem.GetGameItemSO(),
                gameItemGameObject = gameItem.gameObject
            });
            //Debug.Log("Player picked up " + gameItem.GetGameItemSO().name);
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

        transform.position += moveDir.normalized * moveDistance;

        if (lastMoveDir.x < 0)
        {
            transform.forward = Vector3.Slerp(transform.forward, new Vector3(0, 0, 1), Time.deltaTime * rotateSpeed); //turning player to face the direction of movement
        }
        else if (lastMoveDir.x > 0)
        {
            transform.forward = Vector3.Slerp(transform.forward, new Vector3(0, 0, -1), Time.deltaTime * rotateSpeed); //turning player to face the direction of movement
        }
    
    }

    // private void SetSelectedCounter(BaseCounter selectedCounter)
    // {
    //     this.selectedCounter = selectedCounter;

    //     OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
    //     {
    //         selectedCounter = selectedCounter
    //     });
    // }
    public void WriteToGameData()
    {
        GameData.Instance.PlayerFacingDirection = transform.forward;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            transform.forward = GameData.Instance.PlayerFacingDirection;
        }
    }
}
