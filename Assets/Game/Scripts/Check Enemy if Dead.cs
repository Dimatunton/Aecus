using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckEnemyifDead : MonoBehaviour
{
    public List<SimpleEnemyAI> enemies = new List<SimpleEnemyAI>();

    public UnityEvent onAllEnemyDead;

    float timer = 0f;

    void Update()
    {
        if(timer < .1f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            bool allDead = true;
            timer = 0f;
            foreach(SimpleEnemyAI enemy in enemies)
            {
                if(enemy != null)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                onAllEnemyDead.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
