using UnityEngine;
using System.Collections;

public class randomizer : MonoBehaviour {
    [SerializeField]
    AudioClip[] pain;
    [SerializeField]
    AudioClip[] death;
	// Use this for initialization
	void Start () {
	    
	}
	
    public AudioClip GetPain()
    {
        int rand = Random.Range(0, pain.Length);
        return pain[rand];
    }

    public AudioClip GetDeath()
    {
        int rand = Random.Range(0, death.Length);
        return death[rand];
    }
    // Update is called once per frame
    void Update () {
	
	}
}
