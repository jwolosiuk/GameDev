using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SunderTest : MonoBehaviour {

    Sounder mic;
    SpriteRenderer sprite;
    enum States { micNotRecording, waitingForAudio};
    States state;


    [Range(0, 1)]
    public float treshold;

	void Start () {
        //Don't add sounder to every object that needs it
        //Better attach it to some kind of GameManager and do Events
        mic = GetComponent<Sounder>();
        sprite = GetComponent<SpriteRenderer>();
        state = States.micNotRecording;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == States.micNotRecording)
        {
            if (mic.isRecording)
                state = States.waitingForAudio;
        }
        if(state == States.waitingForAudio)
        {
            if(mic.GetAveragedVolume() > treshold)
                sprite.color = Color.red;   
            else
                sprite.color = Color.white;
        }

	}
}
