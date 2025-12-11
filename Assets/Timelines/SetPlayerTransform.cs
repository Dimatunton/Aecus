using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerTransform : MonoBehaviour
{
    public Transform playerTransform;
    public Transform targetTransform;

    private void OnEnable()
    {
        playerTransform.position = targetTransform.position;
        playerTransform.rotation = targetTransform.rotation;
    }

}
