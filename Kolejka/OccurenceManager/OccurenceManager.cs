using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameObjectCycle
{
    public float spaceBarWaitTime;
    public GameObject occurence;
    public GameObject positiveGratification;
    public GameObject negativeGratification;
    public GameObject culmination;
}

public struct Cycle
{
    public Occurence occurence;
    public Gratification positiveGratification;
    public Gratification negativeGratification;
    public Culmination culmination;

    public Cycle(GameObjectCycle gOCycle) {
        this.occurence = gOCycle.occurence.GetComponent<Occurence>();
        this.positiveGratification = gOCycle.positiveGratification.GetComponent<Gratification>();
        this.negativeGratification = gOCycle.negativeGratification.GetComponent<Gratification>();
        this.culmination = gOCycle.culmination.GetComponent<Culmination>();
    }
}

public class OccurenceManager : MonoBehaviour {
    public GameObjectCycle defaultFailGOCycle;
    public GameObjectCycle idleGOCycle;

    public static int cyclesDone = 0;

    public GameObjectCycle[] gOCycles;
    public int actualPlaceNo;
    GameObjectCycle actualCycle;
    public float MIN_WAITING_TIME;
    public float MAX_WAITING_TIME;
    Queue queue;

    public void Init () {
        GameManager.eventSystem.Subscribe("Start", WaitForOccurence);
        GameManager.eventSystem.Subscribe("DoCycle", WaitForOccurence);
        GameManager.eventSystem.Subscribe("Succeeded", Succeeded);
        GameManager.eventSystem.Subscribe("Failed", Failed);
        GameManager.eventSystem.Subscribe("EmptyPlace", EmptyPlace);

        queue = GameManager.queue;
    }
	
	void Update () {
        actualPlaceNo = GameManager.player.GetActualPlaceNumber();
    }

    void WaitForOccurence(EventInfoS e)
    {
        float randWaitingTime = GameManager.instance.RandomRange(MIN_WAITING_TIME, MAX_WAITING_TIME);
        Invoke("PlayOccurenceAtMyPlace", randWaitingTime);
    }

    void PlayOccurenceAtMyPlace()
    {
        actualCycle = gOCycles[actualPlaceNo];
        
        GameObject occurence = Instantiate(actualCycle.occurence);
        occurence.GetComponent<Occurence>().Play();
    }

    void Failed(EventInfoS e)
    {
        defaultFailGOCycle.negativeGratification = actualCycle.negativeGratification;
        SetGOCycleByPlace(defaultFailGOCycle, actualPlaceNo);
        PlayNegativeGratification();
        DoCycle();
    }

    void EmptyPlace(EventInfoS e)
    {
        PlayCulmination();
        GameManager.player.AllowMovementForTime(actualCycle.spaceBarWaitTime);
    }

    void Succeeded(EventInfoS e)
    {
        PlayPositiveGratification();
        DoCycle();
    }

    void DoCycle()
    {
        cyclesDone++;
        GameManager.eventSystem.Call("DoCycle", GameManager.eventSystem);
    } 

    void PlayPositiveGratification()
    {

        GameObject grat = Instantiate(actualCycle.positiveGratification,Vector3.zero,Quaternion.identity);
        grat.GetComponent<Gratification>().Play();
    }

    void PlayNegativeGratification()
    {
        Instantiate(actualCycle.negativeGratification).GetComponent<Gratification>().Play();
    }

    void PlayCulmination()
    {
        Instantiate(actualCycle.culmination).GetComponent<Culmination>().Play();
    }


    public void SetGOCycleByPlace(GameObjectCycle gOCycle, int place)
    {
        gOCycles[place] = gOCycle;
        if(place==actualPlaceNo)
        {
            actualCycle = gOCycle;
        }
    }

    public GameObjectCycle GetGOCycleByPlace(int place)
    {
        return gOCycles[place];
    }

    public void SetActualCycleWaitTime(float time)
    {
        GameObjectCycle gOCycle = gOCycles[actualPlaceNo];
        gOCycle.spaceBarWaitTime = time;
        SetGOCycleByPlace(gOCycle, actualPlaceNo);
    }

    public float GetActualCycleWaitTime()
    {
        return actualCycle.spaceBarWaitTime;
    }
}
