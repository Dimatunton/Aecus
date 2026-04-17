// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSonarShader_ExampleConfigureChildren : MonoBehaviour {

    public Material Sonarmaterial;

    private void Start()
    {
        //foreach (Collider col in GetComponentsInChildren<Collider>(true))
        //{
        //    col.gameObject.AddComponent<SimpleSonarShader_ExampleCollision>();
        //}

        //foreach (Renderer rend in GetComponentsInChildren<Renderer>(true))
        //{
        //    Texture mainTex = rend.material.mainTexture;
        //    rend.material = Sonarmaterial;
        //    rend.material.mainTexture = mainTex;
        //}

        foreach (Renderer rend in GetComponentsInChildren<Renderer>(true))
        {
            Material[] mats = rend.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                Texture mainTex = mats[i].GetTexture("_MainTex");
                Color color = mats[i].color;

                Material newMat = new Material(Sonarmaterial);
                newMat.SetTexture("_MainTex", mainTex);
                newMat.color = color;

                mats[i] = newMat;
            }

            rend.materials = mats;
        }

    }

}
