using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Obstacle
{
    void OnCollisionEnter(Collision collision);
    GameObject Spawn(float posX, float lowerY, float upperY, float posZ);
}
