using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForwardRotate : MonoBehaviour
{
    public InputActionReference moveAction;
    public Transform cameraTransform;

    public float rotationSpeed = 120f; // degrees per second
    public float deadZone = 0.2f;
    public float maxAngle = 45f;

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // --- Flatten directions (Y only)
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 currentForward = transform.forward;
        currentForward.y = 0f;
        currentForward.Normalize();

        // --- Signed angle (so we know left/right)
        float angle = Vector3.SignedAngle(currentForward, camForward, Vector3.up);

        bool joystickTurning = input.magnitude > deadZone;
        bool exceededAngle = Mathf.Abs(angle) > maxAngle;

        if (joystickTurning || exceededAngle)
        {
            // Only rotate the "extra" part beyond maxAngle
            float targetAngle = angle;

            if (!joystickTurning && exceededAngle)
            {
                // Clamp so we only correct excess
                targetAngle = Mathf.Sign(angle) * (Mathf.Abs(angle) - maxAngle);
            }

            // Smooth step rotation (no snapping)
            float step = rotationSpeed * Time.deltaTime;
            float rotationThisFrame = Mathf.Clamp(targetAngle, -step, step);

            transform.Rotate(0f, rotationThisFrame, 0f);
        }
    }
}