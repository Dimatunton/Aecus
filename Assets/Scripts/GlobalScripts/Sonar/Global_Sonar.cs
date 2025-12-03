using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Sonar : MonoBehaviour
{
    [SerializeField] GameObject sonarPrefab;
    
    static GameObject sonar;

    private void Awake()
    {
        sonar = sonarPrefab;
    }

    public static void spawnSonar(Vector3 position, Vector3 direction, float lifeSpan = 1f, float maxSize = 1f)
    {
        Sonar s = Instantiate(sonar, position, Quaternion.LookRotation(direction)).GetComponent<Sonar>();

        s.lifeSpan = lifeSpan;
        s.maxSize = maxSize;
    }


}
