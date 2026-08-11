using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [SerializeField] GameObject CameraOff;
    [SerializeField] GameObject CameraOn;

    private void OnTriggerEnter(Collider other)
    {
        CameraOff.SetActive(false);
        CameraOn.SetActive(true);
    }
}
