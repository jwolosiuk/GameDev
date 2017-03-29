using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class TimeWarp : Occurence {

    enum States { NotPlaying, Playing, Finished, Spawn,Die, NumOfStates};
    States state = States.NotPlaying;

    public float warpPower;
    public float timeBeforeWarp;

    float timeStarted;
    Twirl twirl;

    float epsilon = 5f;
    int rand = 0;
    bool animationDone = false;
    bool spawnDone = false;
    bool soundPlayed = false;


    private void Awake()
    {
        state = States.NotPlaying;
        twirl = GameManager.mainCamera.GetComponent<Twirl>();
        GameManager.eventSystem.Subscribe("Success", Destroy);
        GameManager.eventSystem.Subscribe("Failed", Failed);
    }

    public override void Play()
    {
        if (GameManager.queue.queueLength - 2 > 0)
            rand = GameManager.instance.RandomRange(0, GameManager.queue.queueLength - 2);
        else
            rand = 0;

        state = States.Playing;
        timeStarted = Time.time;
        twirl.center = GameManager.mainCamera.GetComponent<Camera>().WorldToViewportPoint(GameManager.queue[rand].transform.position);

    }

    void Failed(EventInfoS e)
    {
        GameManager.eventSystem.Unsubscribe("Success", Destroy);
        GameManager.eventSystem.Unsubscribe("Failed", Failed);

        twirl.center = GameManager.mainCamera.GetComponent<Camera>().WorldToViewportPoint(GameManager.player.GetNextPlacePosition());
        GameManager.instance.PlaySound("TimeTravel2");
        state = States.Spawn;
    }
    void Destroy(EventInfoS e)
    {
        GameManager.eventSystem.Unsubscribe("Success", Destroy);
        GameManager.eventSystem.Unsubscribe("Failed", Failed);

        Destroy(gameObject);
    }

    private void Update()
    {
        if(state == States.Playing)
        {
            if(Time.time - timeStarted > timeBeforeWarp)
            {
                if(!soundPlayed)
                {
                    GameManager.instance.PlaySound("TimeTravel2");
                    soundPlayed = !soundPlayed;
                }
                if (twirl.angle - warpPower * Time.deltaTime < epsilon)
                {
                    twirl.angle = 360;
                    GameManager.queue.ErasePerson(rand);
                    state = States.Finished;
                }
                else
                {
                    twirl.angle = twirl.angle - (warpPower * Time.deltaTime);
                    if (Mathf.Abs(twirl.angle - 180f) < epsilon)
                        if(GameManager.queue[rand] != null)
                        {
                            if (!animationDone)
                            {
                                MovePlayer mover = GameManager.queue[rand].AddComponent<MovePlayer>();
                                mover.animationFrames = new MovePlayer.SimpleAniStep[1];
                                mover.animationFrames[0] = new MovePlayer.SimpleAniStep(GameManager.queue[rand].transform.position, 0, 2, 0);
                                mover.destroyAtTheEnd = true;
                                mover.Play();
                                animationDone = !animationDone;
                            }
                        }                    
                }
            }
        }
        if(state == States.Spawn)
        {
                if (twirl.angle - warpPower * Time.deltaTime < epsilon)
                {
                    twirl.angle = 360;
                    state = States.Die;
                }
                else
                {
                    twirl.angle = twirl.angle - (warpPower * Time.deltaTime);
                    if (Mathf.Abs(twirl.angle - 180f) < epsilon)
                            if (!spawnDone)
                            {
                                GameManager.queue.AddPerson();
                                MovePlayer mover = GameManager.queue[GameManager.queue.queueLength-1].AddComponent<MovePlayer>();
                                mover.animationFrames = new MovePlayer.SimpleAniStep[2];
                                mover.animationFrames[0] = new MovePlayer.SimpleAniStep(GameManager.queue[GameManager.queue.queueLength - 1].transform.position, 0, 100, 0);
                                mover.animationFrames[1] = new MovePlayer.SimpleAniStep(GameManager.queue[GameManager.queue.queueLength - 1].transform.position, 1, 2, 0);
                                mover.destroyAtTheEnd = false;
                                mover.Play();
                                spawnDone = !spawnDone;
                            }
                }
         }
        if (state == States.Die)
            Destroy(gameObject);

    }

}
