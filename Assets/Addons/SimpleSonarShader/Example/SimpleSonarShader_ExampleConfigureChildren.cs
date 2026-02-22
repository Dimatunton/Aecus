// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSonarShader_ExampleConfigureChildren : MonoBehaviour {

    public Material SonarMaterial;

    private void Start()
    {
        // this is the default Oncollision code
        //foreach(Collider col in GetComponentsInChildren<Collider>(true))
        //{
        //    col.gameObject.AddComponent<SimpleSonarShader_ExampleCollision>();
        //}

        foreach(Renderer rend in GetComponentsInChildren<Renderer>(true))
        {
            Texture mainTex = rend.material.mainTexture;
            rend.material = SonarMaterial;
            rend.material.mainTexture = mainTex;
        }
    }

}
