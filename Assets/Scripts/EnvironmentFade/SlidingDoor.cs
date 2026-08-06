using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{

    [SerializeField] private Animator slidingDoor1 = null;
    [SerializeField] private Animator slidingDoor2 = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && slidingDoor1 && slidingDoor2)
        {
            slidingDoor1.Play("SlidingDoorOpen");
            slidingDoor2.Play("SlidingDoorOpen2");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && slidingDoor1 && slidingDoor2)
        {
            slidingDoor1.Play("SlidingDoorClose");
            slidingDoor2.Play("SlidingDoorClose2");
        }
    }
}
