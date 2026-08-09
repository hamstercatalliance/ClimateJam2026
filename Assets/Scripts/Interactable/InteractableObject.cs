using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class InteractableObject : MonoBehaviour
{
    public string name { get; protected set; }
    public string id { get; protected set; }

    [SerializeField] public InteractableSO scriptableInteractable;
    //public virtual InteractableSO ScriptableInteractableProperty { get { return scriptableInteractable; } set { scriptableInteractable = value; } }

    private GameInput gameInput;

    protected Conversation conversation;

    protected GameObject interactNotice;

    private Player player;

    private void Start()
    {
        interactNotice = Instantiate(Resources.Load<GameObject>("Prefabs/InteractNotice"), transform);
        interactNotice.SetActive(false);
        player = FindFirstObjectByType<Player>();
        player.GetComponent<PlayerInteract>().InteractableActivate += PlayerInteract_InteractableActivate;
        conversation = FindFirstObjectByType<Conversation>();
        name = scriptableInteractable.interactableName;
        id = scriptableInteractable.interactableId;
        gameInput = FindObjectOfType<GameInput>();
        //player = FindFirstObjectByType<Player>();
        Init();
    }
    private void PlayerInteract_InteractableActivate(object sender, System.EventArgs e)
    {
        InteractableActivateEventArgs interactableArgs = e as InteractableActivateEventArgs;
        if (interactableArgs.npcID == scriptableInteractable.interactableId)
        {
            OnTalk();
        }
    }

    protected virtual void OnDestroy()
    {
        if (player != null)
        {
            player.GetComponent<PlayerInteract>().InteractableActivate -= PlayerInteract_InteractableActivate;
        }
    }

    private void FixedUpdate()
    {
        if (getDistance() <= scriptableInteractable.interactionRadius)
        {
            ZoneEntered();
        }
        else
        {
            if (interactNotice != null)
            {
                interactNotice.SetActive(false);
            }
        }
    }

    protected virtual void ZoneEntered()
    {
        //Debug.Log("Player is in interaction zone of " + name);
        if (interactNotice != null)
        {
            interactNotice.SetActive(true);
        }
    }

    public float getDistance() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance;
    }


    protected abstract void OnTalk();

    protected virtual void Init() { }
}
