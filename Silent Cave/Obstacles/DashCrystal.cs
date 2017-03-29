using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCrystal : DestructableCrystal {



    override public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GameManager.player.state == Player.States.Dash || GameManager.player.state == Player.States.DashJump)
            {
                GameManager.instance.PlaySound("CrystalDead");
                DestroyMe();
            }
            else
                GameManager.eventSystem.Call("PlayerDied", GameManager.eventSystem);
        }
    }

}

