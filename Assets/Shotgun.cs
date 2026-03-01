using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shotgun : MonoBehaviour
{
    public Transform gunpont = null;
    public SimpleSonarShader_Parent sonarScript = null;
    public float ShotgunSpread = .2f;
    public int bulletCount = 2;

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
            transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }
    }

    public void selected()
    {
        isGrabbed = true;
    }
    public void unselected()
    {
        if(grabComponent.interactorsSelecting.Count < 1)
        {
            isGrabbed = false;
        }
    }

    public void activated()
    {
        if(bulletCount > 0)
        {
            print(bulletCount);
            for (int i = 0; i < 3; i++)
            {
                float tempShotgunSpread = ShotgunSpread;
                if (grabComponent.interactorsSelecting.Count < 2)
                {
                    tempShotgunSpread = ShotgunSpread + .5f;
                }

                float xrandom = Random.Range(-tempShotgunSpread, tempShotgunSpread);
                float yrandom = Random.Range(-tempShotgunSpread, tempShotgunSpread);
                float zrandom = Random.Range(-tempShotgunSpread, tempShotgunSpread);
                RaycastHit hit;

                Physics.Raycast(new Vector3(gunpont.position.x + xrandom, gunpont.position.y + yrandom, gunpont.position.z + zrandom), gunpont.forward, out hit, 25f);

                if (!sonarScript)
                {
                    Debug.LogError("No Sonar Script assigned to the shotgun!");
                    return;
                }
                sonarScript.StartSonarRing(hit.point, 3f);
            }
            bulletCount--;
        }
    }

    public bool reload()
    {
        if(bulletCount > 2)
        {
            return false;
        }
        else
        {
            bulletCount++;
            return true;
        }
    }
}
