using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : Culmination {

    public override void Play()
    {
        GameObject.Find("New Sprite").GetComponent<SpriteRenderer>().enabled = true;
        Invoke("Destroy", 1.7f);
    }

    void Destroy()
    {
        GameObject.Find("New Sprite").GetComponent<SpriteRenderer>().enabled = false;
    }
}
