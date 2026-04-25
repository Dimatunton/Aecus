using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarScan : MonoBehaviour
{
    public float scanRange = 5f;

    SimpleSonarShader_Parent sonarScript;
    SphereCollider sphereCollider;

    float rad = 0f;
    private void OnEnable()
    {
        rad = 0f;
        sonarScript = SimpleSonarShader_Parent.Instance;
        sphereCollider = GetComponent<SphereCollider>();
        sonarScript.StartSonarRing(transform.position, scanRange);
    }
    void Update()
    {
        if (rad < scanRange)
        {
            rad += Time.deltaTime;
            sphereCollider.radius = rad;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OutlineController OC;
        if (other.TryGetComponent<OutlineController>(out OC))
        {
            OC.outlineActivated = false;
            OC.outlineActivated = true;
            OC.outlineLingerDuration = 3f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rad);
    }

}
