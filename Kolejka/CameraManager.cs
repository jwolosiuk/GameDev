using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    float shake;
    float diminishShake;
    Vector3 initialPos;

    // Use this for initialization
    void Start()
    {
        shake = 0;
        diminishShake = 10;
        initialPos = transform.position;
    }

    public void AddShake(int magnitude)
    {
        shake += magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (shake > 1)
        {
            Vector3 shakeVec;
            if (GameManager.instance != null)
                shakeVec = new Vector3(GameManager.instance.RandomRange(-1 * shake / diminishShake, shake / diminishShake), GameManager.instance.RandomRange(-1 * shake / diminishShake, shake / diminishShake));
            else
                shakeVec = new Vector3(Random.Range(-1 * shake / diminishShake, shake / diminishShake),Random.Range(-1 * shake / diminishShake, shake / diminishShake));
            gameObject.transform.position = Vector3.Lerp(initialPos, shakeVec, Time.deltaTime);
            shake *= 0.97f;
        }
    }
}
