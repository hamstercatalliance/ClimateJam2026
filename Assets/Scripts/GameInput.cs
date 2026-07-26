using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnMenuAction;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Debug.Log("GameInput Awake");
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.MenuToggle.performed += MenuToggle_performed;
        playerInputActions.Player.Interact.performed += Interact_performed;

        //playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }
    private void MenuToggle_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!DialogueBox.dialogueActive || !MerchantStore.merchantStoreOpen || !BoardManager.jobBoardActive)
        {
            OnMenuAction?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!DialogueBox.dialogueActive && !MerchantStore.merchantStoreOpen && !BoardManager.jobBoardActive)
        {
            OnJumpAction?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //if (OnInteractAction != null)
        //{
        //    OnInteractAction(this, EventArgs.Empty);
        //}
        OnInteractAction?.Invoke(this, EventArgs.Empty); //same as above but more compact 
        Debug.Log("GameInput: Interact action triggered");
        //send out event ^
    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized()
    {
        //Debug.Log(playerInputActions);
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        if (DialogueBox.dialogueActive || MerchantStore.merchantStoreOpen || BoardManager.jobBoardActive) {
            return Vector2.zero;
        }
        return inputVector;
    }
}
