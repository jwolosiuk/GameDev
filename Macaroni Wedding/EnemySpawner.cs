using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    int Level = 1;
    [SerializeField]
    GameObject EnemyFigure;

    List<GameObject> enemies;

    public List<GameObject> Enemies
    { get { return enemies; } }

    [SerializeField]
    float Radius = 4f;
    [SerializeField]
    float AngleRange = 90;
    [SerializeField]
    float Offset = 90;

    [SerializeField]
    float Cooldown = 0.5f;

    float Timer = 0f;


	// Use this for initialization
	void Start ()
    {
        enemies = new List<GameObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Timer += Time.deltaTime;
        if(Timer >= Cooldown)
        {
            Timer = 0f;
            Spawn();
        }
	}

    public void ChangeLevel(int level)
    {
        Cooldown *= 0.95f;
    }

    void Spawn()
    {
        float Angle = Random.value * AngleRange * Mathf.Deg2Rad + Offset * Mathf.Deg2Rad;
        Vector3 SpawnPosition = new Vector3(Mathf.Sin(Angle),0,Mathf.Cos(Angle));
        SpawnPosition *= Radius;
        GameObject Temp = (GameObject)Instantiate(EnemyFigure, transform.position + SpawnPosition, Quaternion.identity);
        enemies.Add(Temp);

    }
}
