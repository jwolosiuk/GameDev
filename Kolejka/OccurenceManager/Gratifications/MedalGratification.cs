using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MedalGratification : Gratification {

    public string[] achievements;

    public float textEnterSpeed = 1;
    [Range(0, 360)]
    public int rotationRange = 0;
    public float firstBreakDuration = 0;
    public float devideInStep = 0;
    public float substractInStep = 0;
    public float waitToDie = 5;
    public Vector3 spawnPosition;
    public GameObject achivPrefab;
    public string playSound;

    public bool debug = false;

    float lastTimePlayed = 0f;
    float breakDuration;
    int timesPlayed = 0;
    Transform canvas;

    enum States { NotPlaying, Playing, Die, numOfStates };
    States state = States.NotPlaying;

    void Awake () {
        state = States.NotPlaying;
        timesPlayed = 0;
        breakDuration = firstBreakDuration;
        canvas = transform.Find("Canvas"); // Not sure if returnig correct GameObject 

        if (debug)
            Play();
        
	}
	
	void Update () {
		if(state == States.Playing)
        {
            if(Time.time - lastTimePlayed > breakDuration)
            {
                lastTimePlayed = Time.time;
                if (devideInStep > 0)
                    breakDuration /= devideInStep;
                breakDuration -= substractInStep;

                if (timesPlayed < achievements.Length) //Not sure
                    CreateAchievement(achievements[timesPlayed++]);
                else
                {
                    state = States.Die;
                    lastTimePlayed = Time.time;
                }
            }
        }
        if (state == States.Die)
            if(Time.time - lastTimePlayed > waitToDie)
                Destroy(gameObject);
	}

    public override void Play()
    {
        if (playSound != "")
            GameManager.instance.PlaySound(playSound);
        lastTimePlayed = 0;
        state = States.Playing;
    }

    void CreateAchievement(string s)
    {
        Achievement achiv = Instantiate(achivPrefab, canvas, false).GetComponent<Achievement>();

        float rotation = GameManager.instance.RandomRange((float)-rotationRange, rotationRange);
        achiv.text = s;
        achiv.enterTime = textEnterSpeed;
        achiv.transform.Rotate(new Vector3(0, 0, rotation));
        achiv.Play();
    }

}


