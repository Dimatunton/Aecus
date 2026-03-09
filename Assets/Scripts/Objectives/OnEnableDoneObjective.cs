using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneObjectives : MonoBehaviour
{
    public ObjectivesManager ObjectivesManager;
    public string[] objectList;

    private void OnEnable()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.doneObjective(objectList[i]);
        }
    }
}
