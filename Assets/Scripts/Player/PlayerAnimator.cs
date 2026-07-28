using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_JUMPING = "IsJumping";
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.IsGrounded)
        {
            animator.SetBool(IS_WALKING, Player.Instance.IsWalking);
            animator.SetBool(IS_JUMPING, false);
        }
        else
        {
            animator.SetBool(IS_WALKING, false);
            animator.SetBool(IS_JUMPING, true); 
        }
    }
}