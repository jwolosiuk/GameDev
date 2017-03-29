using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour {


    public string text;
    public float enterTime;
    public int shakePower;
    public float waitToDie = 4f;

    enum States { NotPlaying, Playing, Die, numOfStates };
    States state = States.NotPlaying;

    float timeStarted;

    public void Play()
    {
        state = States.Playing;
        Text _text = GetComponentInChildren<Text>() ;
        _text.text = text;
        GetComponent<Animator>().SetFloat("AnimationSpeed", enterTime);
        GetComponent<Animator>().SetTrigger("Start");
        timeStarted = Time.time;
    }

    private void Update()
    {
        if(state == States.Playing)
        {
            if(Time.time - timeStarted > 1/enterTime)
            {
                //Play Sound
                //Wait smth 
                //Die after ypu wait 
                GetComponent<AudioSource>().Play();
                GameManager.cameraManager.AddShake(shakePower);
                state = States.Die;
                timeStarted = Time.time;
            }
        }
        if (state == States.Die)
            if(Time.time - timeStarted > waitToDie)
                Destroy(gameObject);
    }

}
