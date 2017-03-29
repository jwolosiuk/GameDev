using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingWall : CeilingCrystal
{
    public float MIN_SCALE_Y = 1;
    public float MAX_SCALE_Y = 3;

    override public Vector3 GenerateEulerRotation(Vector3 euler)
    {
        euler.z = Random.Range(-12.0f, 12.0f);
        return euler;
    }

    override public Vector3 GenerateScale()
    {
        float scaleX = Random.Range(MIN_SCALE_X, MAX_SCALE_X);
        float scaleY = Random.Range(MIN_SCALE_Y, MAX_SCALE_Y);

        return new Vector3(scaleX, scaleY, 1);
    }

}
