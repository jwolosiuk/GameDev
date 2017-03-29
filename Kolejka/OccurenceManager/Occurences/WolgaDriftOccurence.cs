using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolgaDriftOccurence : Occurence
{
    public GameObject car;
    GameObject autko, persona;

    public override void Play()
    {
        persona = GameManager.queue[0];
        Animator animator = persona.GetComponent<Animator>();
        autko = Instantiate(car, new Vector3(-10, -3), Quaternion.identity, transform);
        StartCoroutine("DeadCoroutine", autko);
        autko.GetComponent<Animator>().SetBool("Running", true);
    }

    IEnumerator DeadCoroutine(GameObject obj)
    {
        float delay = 0f;

        Vector3 pos = obj.transform.position,
            personaPos = new Vector3 (persona.transform.position.x, obj.transform.position.y);
        while (true)
        {
            delay += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(pos, personaPos, 2.15f*(Mathf.Sqrt(1 + delay)-1));
            if (delay>2)
            {
                GameManager.queue.ErasePerson(0);
                Destroy(persona);
                GetComponent<AudioSource>().Play();
                Invoke("Next", 1.5f);
                yield break;
            }
            yield return null;
        }
    }

    void Next()
    {
        StartCoroutine("DriveAway");
    }

    IEnumerator DriveAway()
    {
        float delay = 0f;
        Vector3 pos = autko.transform.position;
        while (true)
        {
            delay += Time.deltaTime;
            autko.transform.position = Vector3.Lerp(pos, pos + new Vector3(8,0), delay);
            if (delay > 1)
            {
                yield break;
            }
            yield return null;
        }
    }

        void Destroy(EventInfoS e)
    {
        Destroy(gameObject);
    }
}
