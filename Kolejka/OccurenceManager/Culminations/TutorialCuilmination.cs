using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCuilmination : Culmination {


    public float waitTime; 

    public override void Play()
    {
        GameManager.eventSystem.Subscribe("Succeeded", Destroy);
        GameManager.occurenceManager.SetActualCycleWaitTime(waitTime);

    }

   
    void Destroy(EventInfoS e)
    {
        GameManager.eventSystem.Unsubscribe("Succeeded", Destroy);
        Destroy(gameObject);
    }

}
