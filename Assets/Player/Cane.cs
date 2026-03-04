using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cane : MonoBehaviour
{
    public SimpleSonarShader_Parent sonarScript = null;

    private Rigidbody rb;
    private bool isGrabbed = false;
    private XRGrabInteractable grabComponent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabComponent = GetComponent<XRGrabInteractable>();
    }

    private void FixedUpdate()
    {
        if (!isGrabbed)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
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
       
        if(grabComponent.interactorsSelecting.Count > 0)
        {
            sonarScript.StartSonarRing(collision.contacts[0].point, 10f);
        }
    }
}
