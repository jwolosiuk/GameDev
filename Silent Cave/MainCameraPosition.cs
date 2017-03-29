using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraPosition : MonoBehaviour {

    bool movementAllowed;
    Vector3 offset;
    float CAVE_MIDDLE = 0.4f;

    private void Start()
    {
        movementAllowed = false;
        GameManager.eventSystem.Subscribe("GameInitialized", AllowMovement);
        GameManager.eventSystem.Subscribe("PlayerDied", StopMovement);
    }

    private void Update()
    {
        if(movementAllowed)
        {
            Vector3 player = GameManager.player.transform.position;
            player.y =CAVE_MIDDLE;
            gameObject.transform.position = player + offset;            
        }
    }

    void AllowMovement(EventInfoS e)
    {
        offset = gameObject.transform.position - GameManager.player.transform.position;
        movementAllowed = true;
    }
    void StopMovement(EventInfoS e)
    {
        movementAllowed = false;
    }
}
