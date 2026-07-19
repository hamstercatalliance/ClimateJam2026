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

    [SerializeField] private GameInput gameInput;

    protected Conversation conversation;

    private Player player;

    private void Start()
    {

        player = FindFirstObjectByType<Player>();
        player.GetComponent<PlayerInteract>().InteractableActivate += PlayerInteract_InteractableActivate;
        conversation = FindFirstObjectByType<Conversation>();
        name = scriptableInteractable.interactableName;
        id = scriptableInteractable.interactableId;
        //player = FindFirstObjectByType<Player>();
        Init();
    }
    private void PlayerInteract_InteractableActivate(object sender, System.EventArgs e)
    {
        
        OnTalk();
    }
    protected abstract void OnTalk();

    protected virtual void Init() { }
}
