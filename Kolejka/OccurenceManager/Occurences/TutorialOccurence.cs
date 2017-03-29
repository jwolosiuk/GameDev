using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOccurence : Occurence {

    public override void Play()
    {
        GameManager.eventSystem.Subscribe("Succeeded", Destroy);
        RemoveFirstPerson();
    }

    void Destroy(EventInfoS e)
    {
        GameManager.eventSystem.Unsubscribe("Succeeded", Destroy);
        Destroy(gameObject);
    }
}
