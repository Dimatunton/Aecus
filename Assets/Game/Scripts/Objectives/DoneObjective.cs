using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneObjective : MonoBehaviour
{
    public string[] objectList;

    public void doneObjective()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.instance.doneObjective(objectList[i]);
            Debug.Log("Done obj:" + objectList[i], gameObject);
        }
    }
}
