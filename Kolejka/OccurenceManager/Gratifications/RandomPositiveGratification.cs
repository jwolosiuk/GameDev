using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositiveGratification : Gratification {
    public GameObject[] randomizablePositiveGratificationGOs;

    public override void Play()
    {
        int rand = GameManager.instance.RandomRange(0, randomizablePositiveGratificationGOs.Length);
        Instantiate(randomizablePositiveGratificationGOs[rand], Vector3.zero, Quaternion.identity).GetComponent<Gratification>().Play();
        Destroy(gameObject);
    }



}
