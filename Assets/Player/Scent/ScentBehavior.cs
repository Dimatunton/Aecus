using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class ScentBehavior : MonoBehaviour
{
    public Transform target;
    public SplineComputer spline;

    NavMeshPath path;

    public async void Start()
    {       
        path = new NavMeshPath();

        int iD = 0;

        for(int i =  0; i < NavMesh.GetSettingsCount(); i++)
        {
            NavMeshBuildSettings currentSettings = NavMesh.GetSettingsByIndex(i);
            if (NavMesh.GetSettingsNameFromID(i) == "Scent")
            {
                iD = currentSettings.agentTypeID;
                return;
            }
        }


        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

        await Scent();
    }

    private async Task Scent()
    {
        await Task.Delay(100);
        
        if (path.status != NavMeshPathStatus.PathInvalid  )
        {
            for (int i = 0; i < path.corners.Length; i++)
                Debug.DrawLine(path.corners[i], i + 1 < path.corners.Length ? path.corners[i + 1] : path.corners[i], Color.green, 1f);

            SplinePoint[] points = new SplinePoint[path.corners.Length];
            for (int i = 0; i < path.corners.Length; i++)
            {
                if(i != 0 || i != path.corners.Length)
                {
                    points[i].position = path.corners[i] + Vector3.up + (Vector3.up * Random.Range(-.5f, .5f));
                }
                else
                {
                    points[i].position = path.corners[i] + Vector3.up;
                }
                
            }

            spline.SetPoints(points);

            float incrementSize = .8f / path.corners.Length;

            for (int i = 0; i < path.corners.Length; i++)
            {
                spline.SetPointSize(i, .1f + (incrementSize * (i + 1)));
                print(.1f + (incrementSize * (i + 1)));
            }
        }
    }
}
