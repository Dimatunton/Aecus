using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class FirstPersonController : MonoBehaviour
{
    //NOTE TO SELF:
    //* This is for New Input System
    //* Check if mouseDelta is being pass to 'getMouseDelta' function
    //* Always check if the 'cam' is right
    //* 'ESC' key to exit first person for editing

    Transform cam; //this transform
    public float MouseSensitivity = 50;
    private Vector2 mouseDelta;
    private Vector3 camRotation;

    bool trackMouseInput = false;

    TrackedPoseDriver trackedPoseDriver;

    bool onMobile = false;

    private void Start()
    {
        if (!(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
        {
            onMobile = true;
            trackedPoseDriver = GetComponent<TrackedPoseDriver>(); 
        }
        cam = transform;
        camRotation = cam.rotation.eulerAngles;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!onMobile)
        {
            camRotation.x -= (mouseDelta.y * MouseSensitivity * Time.deltaTime);
            camRotation.y += (mouseDelta.x * MouseSensitivity * Time.deltaTime);
            camRotation.x = Mathf.Clamp(camRotation.x, -85f, 85);

            cam.rotation = Quaternion.Euler(camRotation);
        }
        else
        {
            trackedPoseDriver.enabled = false;
        }
    }


    public void getMouseDelta(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

}
