using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneObjectives : MonoBehaviour
{
    public string[] objectList;

    private void OnEnable()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.instance.doneObjective(objectList[i]);
            Debug.Log("Done obj:" + objectList[i], gameObject);
        }
    }
}
