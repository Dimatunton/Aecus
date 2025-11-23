using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateTracker : MonoBehaviour
{
    Enemy owner;
    TextMeshProUGUI StateText;

    private void Start()
    {
        owner = transform.parent.GetComponent<Enemy>();
        StateText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if((owner as SimpleEnemy).CurrentState.ToString() != StateText.text)
        {
            print("change");
            StateText.text = (owner as SimpleEnemy).CurrentState.ToString();
            if (owner.playerTransform != null)
            {
                transform.LookAt(owner.playerTransform.position);
            }
        }
    }
}
