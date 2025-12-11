using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveHandler : MonoBehaviour
{
    public UnityEvent onStartObjective;

    public GameObject ObjectivesTemplate;
    Dictionary<string, GameObject> objectivesList = new Dictionary<string, GameObject>();

    private void Start()
    {
        onStartObjective.Invoke();
    }

    public void AddNewObjectives(string objectivesTitle)
    {
        GameObject OT = Instantiate(ObjectivesTemplate, transform);
        OT.GetComponent<TextMeshProUGUI>().text = "* " + objectivesTitle;

        objectivesList.Add(objectivesTitle, OT);
    }
    
    public void ClearObjectives()
    {
        foreach(GameObject ob in objectivesList.Values)
        {
            Destroy(ob);
        }
        objectivesList.Clear();
    }

    public void RemoveObjective(string objectivesTitle)
    {
        Destroy(objectivesList[objectivesTitle]);
        objectivesList.Remove(objectivesTitle);
    }

    public bool CheckObjective(string objectivesTitle)
    {
        return objectivesList.ContainsKey(objectivesTitle);
    }

    public void printObjectives()
    {
        foreach (string objectiveTitle in objectivesList.Keys)
        {
            print(objectiveTitle);
        }
    }
}
