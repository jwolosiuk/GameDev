using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNegativeGratification : Gratification {
    public GameObject[] randomizableNegativeGratificationGOs;

    public override void Play()
    {
        int rand = GameManager.instance.RandomRange(0, randomizableNegativeGratificationGOs.Length);
        Instantiate(randomizableNegativeGratificationGOs[rand], Vector3.zero, Quaternion.identity).GetComponent<Gratification>().Play();
        Destroy(gameObject);
    }
}
