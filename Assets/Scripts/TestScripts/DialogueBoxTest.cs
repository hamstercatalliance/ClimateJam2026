using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogueBox box = new DialogueBox("","",0.0f);

        DialogueRenderer.Render(box);
    }
}
