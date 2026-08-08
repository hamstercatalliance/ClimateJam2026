using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.5f;
    public enum SurfaceType
    {
        Grass,
        Stone,
        Wood,
        Sand
    }
    private SurfaceType currentSurfaceType = SurfaceType.Stone;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        footstepTimer = footstepTimerMax;
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
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, currentSurfaceType);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            currentSurfaceType = SurfaceType.Grass;
        }
        else if (other.CompareTag("Stone"))
        {
            currentSurfaceType = SurfaceType.Stone;
        }
        else if (other.CompareTag("Wood"))
        {
            currentSurfaceType = SurfaceType.Wood;
        }
        else if (other.CompareTag("Sand"))
        {
            currentSurfaceType = SurfaceType.Sand;
        }
    }
}
