using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableDestroyObject : MonoBehaviour
{
    public List<GameObject> Objects = new List<GameObject>();

    private void OnEnable()
    {
        foreach (GameObject obj in Objects)
        {
            Destroy(obj);
        }
    }
}
