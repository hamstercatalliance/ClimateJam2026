using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueRenderer : MonoBehaviour
{
    [SerializeField] GameObject box;

    public static void Render(DialogueBox dialogueObject) {
        Instantiate(box);

    }
}
