using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultOccurence : Occurence {

    Queue queue;
    public float timeToMeet;
    float distanceToMake;
    GameObject walker;
    MovePlayer mover;
    int rand;
    Vector3 begin = new Vector3(7, -0.5f, 0);
    Vector3 end = new Vector3(-8, -0.5f, 0);
    LayerManager layerManager;
    Vector3 offset = new Vector3(0.6f, -0.5f);

    public override void Play()
    {
        GameManager.eventSystem.Subscribe("Succeeded", OnPlayerSuccess);
        GameManager.eventSystem.Subscribe("Failed", OnPlayerFailure);
        queue = GameManager.queue;
        RemoveFirstPerson();
        Vector3 nextPlayerPosition = GameManager.player.GetNextPlacePosition();
        begin.y = nextPlayerPosition.y+offset.y;
        end.y = begin.y;

        rand = GameManager.instance.RandomRange(0, queue.prefab.Length);
        walker = Instantiate(queue.prefab[rand],begin ,Quaternion.identity);

        timeToMeet = (queue.queueLength-2) / queue.movementSpeed + GameManager.occurenceManager.GetActualCycleWaitTime() - 0.6f;
        distanceToMake = Vector3.Distance(walker.transform.position, nextPlayerPosition+offset);
        float speed = distanceToMake / timeToMeet;

        mover = walker.AddComponent<MovePlayer>();
        
        layerManager = walker.AddComponent<LayerManager>();
        layerManager.name2 = "PasserbyToLeft";
        layerManager.flipX = true;
        layerManager.Init();
        layerManager.SetLayerRecursively(layerManager.gameObject, 10);
        walker.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        //Destroy(walker.GetComponent<Rigidbody2D>());
        //Destroy(walker.GetComponentInChildren<CapsuleCollider2D>());


        mover.animationFrames = new MovePlayer.SimpleAniStep[2];
        mover.animationFrames[0] = new MovePlayer.SimpleAniStep(nextPlayerPosition + offset, 1, speed, 0);
        //Invoke("MirrorSprites", 0.2f);
        //float waitingTime = 0.7f;
        //mover.animationFrames[1] = new MovePlayer.SimpleAniStep(new Vector3(nextPlayerPosition.x, nextPlayerPosition.y - yOffset, 0), 0.99f, 0.01f/waitingTime, 0);
        mover.animationFrames[1] = new MovePlayer.SimpleAniStep(nextPlayerPosition, 1, offset.magnitude/0.4f, 0);
        
        mover.Play();
    }

    void Destroy(EventInfoS e)
    {
        Destroy(gameObject);
    }

    void OnPlayerFailure(EventInfoS e)
    {
        GameManager.eventSystem.Unsubscribe("Succeeded", OnPlayerSuccess);
        GameManager.eventSystem.Unsubscribe("Failed", OnPlayerFailure);
        walker.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
        layerManager.flipX = false;
        layerManager.name2 = "Queue";
        layerManager.SetLayerRecursively(layerManager.gameObject, 8);
        layerManager.Init();
        queue.AddPerson(walker);
        Destroy(gameObject);
    }

    void OnPlayerSuccess(EventInfoS e)
    {
        GameManager.eventSystem.Unsubscribe("Succeeded", OnPlayerSuccess);
        GameManager.eventSystem.Unsubscribe("Failed", OnPlayerFailure);
        mover.animationFrames = new MovePlayer.SimpleAniStep[1];
        mover.animationFrames[0] = new MovePlayer.SimpleAniStep(end, 1, 2f, 0);
        mover.destroyAtTheEnd = true;
        mover.Play();
        Destroy(gameObject);
    }

    void MirrorSprites()
    {
        layerManager.flipX = !layerManager.flipX;
        layerManager.Init();
    }
}
