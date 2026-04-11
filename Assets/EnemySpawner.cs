using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int quantity = 5;
    public float spawnInterval = 2f;
    public float spawnRadius = 10f;

    private int spawnedCount = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawnedCount < quantity)
        {
            Vector3 spawnPos = GetRandomNavMeshPosition(transform.position, spawnRadius);
            if (spawnPos != Vector3.zero)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnedCount++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++) // try 30 times to find valid spot
        {
            Vector3 randomPos = center + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero; // failed to find valid position
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}