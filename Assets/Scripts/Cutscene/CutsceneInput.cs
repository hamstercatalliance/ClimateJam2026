using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CutsceneInput : MonoBehaviour
{
    public event EventHandler OnCutsceneProceed;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.CutsceneProceed.performed += CutsceneProceed_Performed;
    }
    private void OnDestroy()
    {
        playerInputActions.Player.CutsceneProceed.performed -= CutsceneProceed_Performed;
        playerInputActions.Player.Disable();
    }
    private void CutsceneProceed_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCutsceneProceed?.Invoke(this, EventArgs.Empty);
    }
}
