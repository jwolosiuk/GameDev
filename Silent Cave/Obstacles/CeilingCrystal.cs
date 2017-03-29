using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCrystal : Crystal {
    override public GameObject Spawn(float posX, float lowerY, float upperY, float posZ)
    {
        return base.Spawn(posX, upperY, 0, posZ);
    }
}
