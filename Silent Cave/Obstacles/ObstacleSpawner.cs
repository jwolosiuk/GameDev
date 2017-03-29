using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    public GameObject[] obstacles;
    float keepObstInRange = 10f;

    public float MIN_DISTANCE = 0.5f;
    public float MAX_DISTANCE = 9f;
    public float[] NO_PASS;
    float posZ = 0.5f;

    float distanceNow = 0;

    public float spawningFreq = 2;

    float lastObstaclePos = 0;

    bool nextWillBeHangingWall = false;

    List<GameObject> existingObstacles = new List<GameObject>();

    List<float> a = new List<float>(new float[] { 1, 1, 1, 1 });
    float c = 0.0001f;

    private void Start()
    {
         NO_PASS = new float[] { 0.6f, 6f };
    }

    public void GenerateObstacleHere(float posX,float lowerY, float upperY)
    {
        int rand = Random.Range(0,obstacles.Length);
        GameObject obstaclePrefab = obstacles[rand];
        Crystal obstacle = obstaclePrefab.GetComponent<Crystal>();

        if(obstacle.MIN_DISTANCE_LEFT!=0)
        {
            RemoveSomeRightObstacles(posX,obstacle.MIN_DISTANCE_LEFT);
        }
        if(obstacle.MIN_DISTANCE_RIGHT!=0)
            distanceNow = obstacle.MIN_DISTANCE_RIGHT;
        
        GameObject instant = obstacle.Spawn(posX, lowerY, upperY, posZ);
        existingObstacles.Add(instant);
    }

    public bool ShouldGenerateHere(float posX)
    {
        if (posX>lastObstaclePos+distanceNow)
        {
            lastObstaclePos = posX;
            distanceNow = GenerateDistance();
            return true;
        }
        return false;
    }

    public void RemoveSomeRightObstacles(float posX, float inDistance)
    {
        if (posX == 0f) return;
        int licz = existingObstacles.Count - 1;
        while (licz >= 0 && posX - existingObstacles[licz].transform.position.x < inDistance)
        {
            Destroy(existingObstacles[existingObstacles.Count - 1]);
            existingObstacles.RemoveAt(existingObstacles.Count - 1);
            licz--;
        }
    }

    public void RemoveSomeLastObstacles(float posX)
    {
        //Igor Dodal
        existingObstacles.RemoveAll(ob => ob == null);
        //Koniec
        while (existingObstacles[0].transform.position.x+keepObstInRange < posX)
        {
            if (existingObstacles[0] != null)
                Destroy(existingObstacles[0]);
            existingObstacles.RemoveAt(0);
        }
   }

   float GenerateDistance()
    {
        float next = a[3] - a[0] * c;
        a.Add(next);
        a.RemoveAt(0);

        float rand = Random.Range(MIN_DISTANCE, MAX_DISTANCE / next);
        if (rand > NO_PASS[0]*next && rand < NO_PASS[1]/next)
            rand = Random.Range(NO_PASS[1], MAX_DISTANCE * next);
        else
            rand = Random.Range(MIN_DISTANCE, NO_PASS[0]);
        return rand;
    }
 
}
