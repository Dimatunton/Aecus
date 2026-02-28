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
            for(int i = 0; i < rend.materials.Length; i++)
            {
                Texture mainTex = rend.materials[i].mainTexture;
                rend.materials[i] = SonarMaterial;
                rend.materials[i].mainTexture = mainTex;
            }
        }
    }

}
