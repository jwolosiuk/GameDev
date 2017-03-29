using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occurence : MonoBehaviour {


    public virtual void Play(){
        Debug.Log("Play occurence");
    }

    void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void RemoveFirstPerson()
    {
        GameObject person = GameManager.queue[0];
        //person odpal animacje 
        MovePlayer mover = person.AddComponent<MovePlayer>();
        Destroy(person.GetComponent<Rigidbody2D>());
        Destroy(person.GetComponentInChildren<CapsuleCollider2D>());
        mover.animationFrames = new MovePlayer.SimpleAniStep[2];
        mover.animationFrames[0] = new MovePlayer.SimpleAniStep(new Vector3(5.7f, -1f, 0), 1, 2, 0);
        mover.animationFrames[1] = new MovePlayer.SimpleAniStep(new Vector3(5.7f, -0.8f, 0), 0, 1f, 0);

        mover.destroyAtTheEnd = true;
        mover.Play();
        GameManager.queue.ErasePerson(0);
    }
}
