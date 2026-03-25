using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OntriggerDoneObjectives : MonoBehaviour
{
    public string gameobjectName;

    public UnityEvent Ontrigger;

    public string[] objectList;


    private void OnTriggerEnter(Collider other)
    {
        if(gameobjectName  == other.gameObject.name)
        {
            Ontrigger.Invoke();
            for (int i = 0; i < objectList.Length; i++)
            {
                ObjectivesManager.instance.doneObjective(objectList[i]);
                Debug.Log("Done obj:" + objectList[i], gameObject);
            }
        }
    }
}
