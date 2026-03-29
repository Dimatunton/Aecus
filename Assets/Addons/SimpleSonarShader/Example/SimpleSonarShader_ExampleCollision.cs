// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using UnityEngine;


public class SimpleSonarShader_ExampleCollision : MonoBehaviour
{
    SimpleSonarShader_Parent parent;

    Coroutine sonartimer;

    public void Start()
    {
        parent = SimpleSonarShader_Parent.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (sonartimer == null)
        {
            sonartimer = StartCoroutine(SonarTimer());
            if (parent) parent.StartSonarRing(transform.position, .5f);
            print("sonar");
        }
        print("collide");
    }

    IEnumerator SonarTimer()
    {
        yield return new WaitForSeconds(.1f);
        sonartimer = null;
    }
}
