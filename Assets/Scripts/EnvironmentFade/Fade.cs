using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    float originalOpacity = 1.0f;
    private MaterialPropertyBlock block;
    Renderer rend;
    [SerializeField]
    private float fadeSpeed = 5f;
    [SerializeField]
    private float fadeAmount = 0.2f;
    public bool fadeNow = false;
    private float currentFade = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeNow)
        {
            currentFade = Mathf.Lerp(currentFade, fadeAmount, fadeSpeed * Time.deltaTime);
        }
        else
        {
            currentFade = Mathf.Lerp(currentFade, originalOpacity, fadeSpeed * Time.deltaTime);
        }
        block.SetFloat("_Fade", currentFade);
        rend.SetPropertyBlock(block);
    }
}
