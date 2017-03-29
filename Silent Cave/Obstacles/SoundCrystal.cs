using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCrystal : DestructableCrystal
{

    override public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.eventSystem.Call("PlayerDied", GameManager.eventSystem);
        }
    }

    
}
