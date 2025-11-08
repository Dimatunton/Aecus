using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmellSkill : MonoBehaviour
{

    public GameObject scent;
    public Material scentMat;
    public float radius = 15f;
    public float lingerTime = 5f;

    float matTransparency = 0f;
    float timer = 0f;

    [SerializeField] LayerMask EnemyLayer;

    List<GameObject> scentGameobjects = new List<GameObject>();



    public void onSmell(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            skillOff();
            Collider[] enemyCol = Physics.OverlapSphere(transform.position, radius, EnemyLayer);

            if (enemyCol.Length > 0)
            {
                foreach (Collider col in enemyCol)
                {
                    GameObject s = Instantiate(scent, transform.position, Quaternion.identity);
                    s.GetComponent<ScentBehavior>().target = col.transform;
                    scentGameobjects.Add(s);
                }
            }
            else
            {
                Debug.Log("no enemy to smell");
            }
        }
    }

    private void Update()
    {
        
        if (timer <= lingerTime)
        {
            timer += Time.deltaTime;
            if (matTransparency < 1f)
            {
                matTransparency += Time.deltaTime;
                scentMat.SetFloat("_OverallAlpha", matTransparency);
            }
        }
        else
        {
            if (matTransparency > 0f)
            {
                matTransparency -= Time.deltaTime;
                scentMat.SetFloat("_OverallAlpha", matTransparency);
            }
            else
            {
                skillOff();
            }
        }

    }
        
    void skillOff()
    {
        scentMat.SetFloat("_OverallAlpha", 0f);
        matTransparency = 0f;
        timer = 0f;

        foreach (GameObject g in scentGameobjects)
        {
            Destroy(g);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, .25f);
        Gizmos.DrawSphere(transform.position, 10f);


    }
}
