using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarOnEnable : MonoBehaviour
{
    public SimpleSonarShader_Parent sonarParent;
    public float sonarIntensity = 5f;

    private void OnEnable()
    {
        sonarParent.StartSonarRing(transform.position, sonarIntensity);
    }
}
