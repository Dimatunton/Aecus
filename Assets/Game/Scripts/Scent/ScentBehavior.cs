using Dreamteck.Splines;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class ScentBehavior : MonoBehaviour
{
    public float scentLingerTimer = 5f;
    private float timer = 0f;

    public Transform target;

    public SplineComputer spline;

    public string agentTypeName = "Humanoid";

    private NavMeshPath navPath;

    private void Start()
    {
        _ = GenerateScentTrailAsync();
    }

    private void Update()
    {
        if(scentLingerTimer >= 0f) 
        {
            timer += Time.deltaTime;
            if (timer > scentLingerTimer)
            {
                Destroy(gameObject);
            }
        }
    }

    private async Task GenerateScentTrailAsync()
{
    navPath = new NavMeshPath();

    int agentTypeId = GetAgentTypeId(agentTypeName);

    Vector3 startPos = transform.position;
    Vector3 endPos = target.position;
    NavMeshHit hit;

    if (NavMesh.SamplePosition(startPos, out hit, 2.0f, NavMesh.AllAreas))
        startPos = hit.position;
    else
        Debug.LogWarning("ScentBehavior: Start position not on NavMesh.");

    if (NavMesh.SamplePosition(endPos, out hit, 2.0f, NavMesh.AllAreas))
        endPos = hit.position;
    else
        Debug.LogWarning("ScentBehavior: Target position not on NavMesh.");

    bool pathFound = NavMesh.CalculatePath(
        startPos,
        endPos,
        agentTypeId >= 0 ? 1 << agentTypeId : NavMesh.AllAreas,
        navPath
    );

    if (!pathFound || navPath.status != NavMeshPathStatus.PathComplete)
    {
        Debug.LogWarning("ScentBehavior: No valid NavMesh path found.");
        return;
    }

    await Task.Delay(100);

    DrawDebugPath(navPath);
    ApplyPathToSpline(navPath);
}

    private int GetAgentTypeId(string typeName)
    {
        for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var settings = NavMesh.GetSettingsByIndex(i);
            if (NavMesh.GetSettingsNameFromID(settings.agentTypeID) == typeName)
                return settings.agentTypeID;
        }
        return -1; // Fallback to all areas if not found
    }

    private void DrawDebugPath(NavMeshPath path)
    {
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green, 1f);
        }
    }

    private void ApplyPathToSpline(NavMeshPath path)
    {
        int count = path.corners.Length;
        if (count < 2)
        {
            Debug.LogWarning("ScentBehavior: Not enough corners to create a spline.");
            return;
        }

        var points = new SplinePoint[count];
        for (int i = 0; i < count; i++)
        {
            Vector3 basePos = path.corners[i] + Vector3.up;
            if (i != 0 && i != count - 1)
            {
                basePos += Vector3.up * Random.Range(-0.5f, 0.5f);
            }
            points[i] = new SplinePoint(basePos);
        }

        spline.SetPoints(points);

        // Optionally, set point sizes for visual effect
        float baseSize = 0.5f;
        float increment = 0.2f / count;
        for (int i = 0; i < count; i++)
        {
            spline.SetPointSize(i, baseSize + increment * i);
        }
    }
}