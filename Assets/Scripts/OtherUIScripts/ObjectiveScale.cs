using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScale : MonoBehaviour
{
    private void Awake()
    {
        transform.position = transform.parent.position + transform.localPosition;
    }
    public Vector3 targetWorldScale = Vector3.one;
    private void LateUpdate()
    {
        if (transform.parent != null)
        {
            transform.localScale = Vector3.Scale(targetWorldScale, new Vector3(1f / transform.parent.lossyScale.x, 1f / transform.parent.lossyScale.y, 1f / transform.parent.lossyScale.z));
            //transform.position = transform.parent.position + transform.localPosition; // Keep the position consistent with the parent
            transform.rotation = Quaternion.identity; // Reset rotation to avoid skewing
        }
        else
        {
            transform.localScale = targetWorldScale;
        }
    }
}
