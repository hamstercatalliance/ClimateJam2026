using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.5f;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            // Play footstep sound
            footstepTimer = footstepTimerMax;
            if (player.IsWalking && player.IsGrounded)
            {
                SoundManager.Instance.PlayFootstepsSound(player.transform.position);
            }
        }
    }
}
