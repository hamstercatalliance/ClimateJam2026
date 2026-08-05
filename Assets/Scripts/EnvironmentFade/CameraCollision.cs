using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    [SerializeField]
    private bool fadeOut = true;
    public GameObject toFade = null; //set this to an object with a fade script, make sure material is using transparent shadergraph
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        }
    }
}
