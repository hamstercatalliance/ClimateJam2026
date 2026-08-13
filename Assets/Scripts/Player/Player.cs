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
    private bool isInTransition = false;
    private bool PortalWalkLeft = false;
    public bool disableMove = false;
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
    public event EventHandler OnPlayerJump;
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
        isInTransition = false;
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
            OnPlayerJump?.Invoke(this, EventArgs.Empty);
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
            PortalWalkLeft = other.gameObject.GetComponent<TriggerSceneLoader>().exitLeft;
            isInTransition = true;
        }
        if (other.gameObject.CompareTag("EndGameTrigger"))
        {
            FinalDaySceneManager.Instance.ShowBlackScreen();
        }
    }
    private void HandleMovement()
    {
            if(disableMove)
        {
            IsWalking = true;
            return;
        }
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            if (moveDir.x != 0)
            {
                lastMoveDir = moveDir;
            }
        if (isInTransition)
        {
            if (PortalWalkLeft)
            {
                moveDir = new Vector3(-1.0f, 0, 0);
            }
            else
            {
                moveDir = new Vector3(1.0f, 0, 0);
            }
        }
        float moveDistance = moveSpeed * Time.deltaTime;
            float playerRadius = .6f;
            float playerHeight = 2f;

            Vector3 castStart = transform.position + Vector3.up * 0.55f;
            Vector3 castEnd = transform.position + Vector3.up * playerHeight;

            // bool canMove = !Physics.CapsuleCast(
            //     transform.position + Vector3.up * 0.6f,
            //     transform.position + Vector3.up * playerHeight,
            //     playerRadius,
            //     moveDir,
            //     moveDistance,
            //     Physics.AllLayers,
            //     QueryTriggerInteraction.Ignore
            // );

            if (moveDir != Vector3.zero)
            {
                if (Physics.CapsuleCast(
                    castStart,
                    castEnd,
                    playerRadius,
                    moveDir,
                    out RaycastHit hit,
                    moveDistance,
                    Physics.AllLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    Vector3 slideDir = Vector3.ProjectOnPlane(moveDir, hit.normal);

                    if (!Physics.CapsuleCast(
                        castStart,
                        castEnd,
                        playerRadius,
                        slideDir.normalized,
                        moveDistance,
                        Physics.AllLayers,
                        QueryTriggerInteraction.Ignore))
                    {
                        transform.position += slideDir.normalized * moveDistance;
                        IsWalking = slideDir != Vector3.zero;
                    }
                    else
                    {
                        IsWalking = false;
                    }
                }
                else
                {
                    transform.position += moveDir * moveDistance;
                    IsWalking = true;
                }
            }
            else
            {
                IsWalking = false;
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
