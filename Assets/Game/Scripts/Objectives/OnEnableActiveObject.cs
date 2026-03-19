using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableActiveObject : MonoBehaviour
{
    public List<GameObject> Objects = new List<GameObject>();

    private void OnEnable()
    {
        foreach (GameObject obj in Objects)
        {
            obj.SetActive(true);
        }
    }
}
