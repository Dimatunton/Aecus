using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddObjectives : MonoBehaviour
{
    public UnityEvent onAdd;
    public string[] objectList;


    public void addObjectives()
    {
        onAdd.Invoke();
        StartCoroutine(addobjs());
    }

    IEnumerator addobjs()
    {
        
        ObjectivesManager.instance.clearObjectives();
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.instance.addObjective(i, objectList[i]);
            Debug.Log("Added obj:" + objectList[i], gameObject);
            yield return new WaitForSeconds(.5f);
        }
    }

}
