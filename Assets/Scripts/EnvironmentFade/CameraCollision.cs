using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public enum CamAnimation
    {
        ZoomIn,
        ZoomOut,
        ResetZoomOut,
        ResetZoomIn,
        None
    }
    [SerializeField] private bool fadeOut = true;
    public GameObject toFade = null; //set this to an object with a fade script, make sure material is using transparent shadergraph
    [SerializeField] private Animator cameraAnim = null;
    [SerializeField] private bool cameraReset = false;
    [SerializeField] CamAnimation SelectCamAnimation = CamAnimation.None;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(toFade != null)
            {
                toFade.GetComponent<Fade>().fadeNow = fadeOut;
            }
            if(cameraAnim != null && !cameraReset)
            {
                switch (SelectCamAnimation)
                {
                    case CamAnimation.ZoomIn:
                        cameraAnim.Play("ZoomIn");
                        break;
                    case CamAnimation.ZoomOut:
                        cameraAnim.Play("ZoomOut");
                        break;
                    case CamAnimation.ResetZoomIn:
                        cameraAnim.Play("ResetFromZoomIn");
                        break;
                    case CamAnimation.ResetZoomOut:
                        cameraAnim.Play("ResetFromZoomOut");
                        break;
                    case CamAnimation.None:
                        break;
                }
            }
            cameraReset = !cameraReset;
        }
    }
}
