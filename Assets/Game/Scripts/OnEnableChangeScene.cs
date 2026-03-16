using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string SceneName;
    private void OnEnable()
    {
        if(SceneName.Length > 0)
        {
            SceneManager.LoadScene(SceneName);
            
        }
        else
        {
            Debug.LogError("No Scene name to load");
        }
        
    }
}
