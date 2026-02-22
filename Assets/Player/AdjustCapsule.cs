using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCapsule : MonoBehaviour
{
    public CharacterController characterController;
    void Update()
    {
        transform.localPosition = new Vector3(characterController.center.x, characterController.center.y - 1.75f, characterController.center.z);
        transform.localScale = new Vector3(characterController.radius * 2, characterController.height / 2, characterController.radius * 2);
    }
}
