using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPassingOccurence : Occurence
{
    public GameObject[] cars;

    [Range(5, 8)]
    public float timeToTravel;

    public override void Play()
    {
        Invoke("Method", 1);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Method()
    {
        GameObject car = cars[GameManager.instance.RandomRange(0, 3)];
        Vector3 pos = new Vector3(-10, -3);
        car = Instantiate(car, pos, Quaternion.identity);
        car.GetComponent<Animator>().SetBool("Running", true);
        StartCoroutine("RideMe", car);
    }

    IEnumerator RideMe(GameObject obj)
    {
        float t = 0f;
        Vector3 pos = obj.transform.position;
        while (true)
        {
            t += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(pos, pos + new Vector3(24, 0), t / timeToTravel);
            if (obj.transform.position == pos + new Vector3(24, 0))
            {
                Invoke("Method", 1);
                yield break;
            }
            yield return null;
        }
    }
}
