using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cane : MonoBehaviour
{
    public SimpleSonarShader_Parent sonarScript = null;

    private Rigidbody rb;
    private bool isGrabbed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isGrabbed)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }
    }
    public void selected()
    {
        isGrabbed = true;
    }

    public void unselected()
    {
        isGrabbed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!sonarScript)
        {
            Debug.LogError("No Sonar Script assigned to the cane!");
            return;
        }
        
            sonarScript.StartSonarRing(collision.contacts[0].point, 4f);
        
        
    }
}
