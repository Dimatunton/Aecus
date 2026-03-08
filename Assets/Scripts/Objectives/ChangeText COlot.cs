using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextCOlot : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Text>().color = Color.green;
    }
}
