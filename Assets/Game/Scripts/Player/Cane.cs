using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cane : MonoBehaviour
{
    public SimpleSonarShader_Parent sonarScript = null;
    public float hugeSonarInterval = 1f;

    private Rigidbody rb;
    private bool isGrabbed = false;
    private XRGrabInteractable grabComponent;

    Coroutine onTapCoroutine = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabComponent = GetComponent<XRGrabInteractable>();
        sonarScript = SimpleSonarShader_Parent.Instance;
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
            if(onTapCoroutine == null)
            {
                onTapCoroutine = StartCoroutine(onTap(collision.contacts[0].point));
            }
            else
            {
                sonarScript.StartSonarRing(collision.contacts[0].point, 5f);
            }
            
        }
    }
    
    IEnumerator onTap(Vector3 contactPoint)
    {
        sonarScript.StartSonarRing(contactPoint, 15f);
        yield return new WaitForSeconds(hugeSonarInterval);
        onTapCoroutine = null;
    }

}
