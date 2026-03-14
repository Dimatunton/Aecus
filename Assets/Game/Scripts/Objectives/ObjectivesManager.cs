using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager instance;

    public void Awake()
    {
        instance = this;
    }

    public List<Text> objList = new List<Text>();

    public void clearObjectives()
    {
        foreach(Text t  in objList)
        {
            t.color = Color.green;
            t.GetComponent<Animator>().Play("Outro");
        }
    }

    public void addObjective(int Index, string ObjectiveDescription)
    {

        objList[Index].color = Color.white;
        objList[Index].text = ObjectiveDescription;
        objList[Index].GetComponent<Animator>().Play("Intro");
    }

    public void doneObjective(string ObjectiveDescription)
    {
        bool objExist = false;
        foreach(Text t in objList)
        {
            if (!objExist && t.text == ObjectiveDescription)
            {
                t.color = Color.green;
                objExist = true;
            }
        }
    }
}
