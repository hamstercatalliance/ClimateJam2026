using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetach : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera mainCamera;
    [SerializeField] private Transform playerToFollow;
    private Animator camAnimator;
    [SerializeField] private bool detach = true;
    // Start is called before the first frame update
    void Start()
    {
        camAnimator = mainCamera.GetComponent<Animator>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (detach)
        {
            mainCamera.Follow = null;
        }
        else
        {
            mainCamera.Follow = playerToFollow;
        }
        detach = !detach;
    }
}