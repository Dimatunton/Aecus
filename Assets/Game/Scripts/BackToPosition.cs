using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BackToPosition : MonoBehaviour
{

    Vector3 origPosition;
    Quaternion origRotation;

    void Start()
    {
        origPosition = transform.position;
        origRotation = transform.rotation;
    }

    public void backToPosition()
    {
        transform.position = origPosition;
        transform.rotation = origRotation;
    }
}
