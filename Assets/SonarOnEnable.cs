using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarOnEnable : MonoBehaviour
{
    public SimpleSonarShader_Parent sonarParent;


    private void OnEnable()
    {
        sonarParent.StartSonarRing(transform.position, 5f);
    }
}
