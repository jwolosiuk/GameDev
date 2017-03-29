using UnityEngine;
using System.Collections;

public class Corpse : MonoBehaviour
{
    int CurrentLevel;
    GameManager Menager;
    [SerializeField]
    GameObject Splat;
    [SerializeField]
    float DestroyTime = 0.5f;
    
	// Use this for initialization
	void Start ()
    {
        GameObject meng = GameObject.FindGameObjectWithTag("GameMenager");
        
        if(meng)
        {
            Menager = meng.GetComponent<GameManager>();
            CurrentLevel = Menager.level;
        }
            
        Instantiate(Splat, gameObject.transform.position,Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Menager)
        if (Menager.level != CurrentLevel)
            InitiateDestroy();
	}

    void InitiateDestroy()
    {
        DestroyTime = Random.value * DestroyTime;
        Destroy(gameObject, DestroyTime);
    }


}
