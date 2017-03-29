using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGratificaton : Gratification {

    public string[] achievements;

    public float textEnterSpeed = 1;
    public float breakDuration = 0;
    public float waitToDie = 5;
    public GameObject achivPrefab;

    public bool debug = false;

    float lastTimePlayed = 0f;
    int timesPlayed = 0;
    Transform canvas;

    enum States { NotPlaying, Playing, Die, numOfStates };
    States state = States.NotPlaying;

    void Awake () {
        state = States.NotPlaying;
        timesPlayed = 0;
        canvas = transform.Find("Canvas"); // Not sure if returnig correct GameObject

        if (debug)
            Play();
    }

	void Update () {
        if (state == States.Playing)
        {
            if (Time.time - lastTimePlayed > breakDuration)
            {
                lastTimePlayed = Time.time;
                if (timesPlayed < achievements.Length) 
                    CreateAchievement(achievements[timesPlayed++]);
                else
                {
                    state = States.Die;
                    lastTimePlayed = Time.time;
                }
            }
        }
        if (state == States.Die)
            if (Time.time - lastTimePlayed > waitToDie)
                Destroy(gameObject);
    }

    public override void Play()
    {
        lastTimePlayed = 0;
        state = States.Playing;
    }

    void CreateAchievement(string s)
    {
        
        Achievement achiv = Instantiate(achivPrefab, canvas, false).GetComponent<Achievement>();
        achiv.text = s;
        achiv.enterTime = textEnterSpeed;
        achiv.Play();
    }
}
