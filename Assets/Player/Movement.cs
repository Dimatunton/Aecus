using Google.XR.Cardboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Transform cam;
    public float SPEED = 2.5f;

    public bool stepSonarEnable = true;
    public bool lightStepEnable = true;
    [SerializeField] GameObject lightStep;
    [SerializeField] float lightStepInterval = .5f;
    int ldirection = 1;
    float lightStepTimer = 0f;

    Vector2 Input;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (Input != Vector2.zero)
        {
            Vector3 newDirection = cam.forward * Input.y + cam.right * Input.x;
            newDirection.y = 0;

            Vector3 newVelocity = newDirection.normalized * SPEED;
            newVelocity.y = rb.velocity.y;
            rb.velocity = newVelocity;

            
            lightStepTimer += Time.deltaTime;
            if (lightStepTimer > lightStepInterval)
            {
                ldirection *= -1;
                Vector3 newPos = transform.position + (new Vector3(cam.right.x, 0f, cam.right.z) * .25f * ldirection) + (new Vector3(cam.forward.x, 0f, cam.forward.z) * .5f) + (Vector3.up * .5f);
                if (lightStepEnable)
                {
                    
                    GameObject l = Instantiate(lightStep, newPos, Quaternion.identity);
                }
                if (stepSonarEnable)
                {
                    Global_Sonar.spawnSonar(newPos, Vector3.up);
                }
                
                lightStepTimer = 0f;
            }
        }
    }

    public void getMovement(InputAction.CallbackContext context)
    {
        Input = context.ReadValue<Vector2>();
    }

    public void Gear(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Api.ScanDeviceParams();
        }
    }
}
