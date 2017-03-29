using UnityEngine;
using System.Collections;

public class BuffSpawner : MonoBehaviour {

    [SerializeField]
    GameObject buff;

    [SerializeField]
    float Cooldown = 2f;

    public float mapWidth = 7;
    public float mapHeight = 5;

    [System.Serializable]
    public struct buff_prob
    {
        public GameObject buff;
        public float wage;
    };
    [SerializeField]
    buff_prob[] tablicaBuffow;

    float Timer = 0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer >= Cooldown)
        {
            Timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject temp = (GameObject)Instantiate(ChooseBuff(), new Vector3(Random.Range(-mapWidth,mapWidth),15, Random.Range(-mapHeight, mapHeight)), Quaternion.identity);

    }

    GameObject ChooseBuff()
    {
        GameObject chosen=null;
        float sumWages = 0;
        for(int i=0;i< tablicaBuffow.Length;i++)
        {
            sumWages += tablicaBuffow[i].wage;
        }
        float rand = Random.Range(0f, sumWages);

        float tempSum=0;
        for(int i=0;i<tablicaBuffow.Length;i++)
        {
            tempSum += tablicaBuffow[i].wage;
            if (rand < tempSum) return tablicaBuffow[i].buff;
            
        }
        return tablicaBuffow[tablicaBuffow.Length-1].buff;
    }
}
