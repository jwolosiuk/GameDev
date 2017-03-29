using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    // Use this for initialization
    List<GameObject> list;

    public GameObject[] prefab;
    public int queueLength;

    public float movementSpeed=0.8f;

    public Vector3 gap;

    int playerPos;
    public Vector3 pos;

    public void Init()
    {
        queueLength = GameManager.instance.GetNumberOfPlaces() - 1;
        pos = transform.position;
        list = new List<GameObject>();
        for (int i = 0; i < queueLength; i++)
        {
            int rand = GameManager.instance.RandomRange(0, prefab.GetLength(0));
            GameObject character = Instantiate(prefab[rand], pos, Quaternion.identity, transform);
            character = GameManager.instance.RandomizeCharacterParameters(character);
            list.Add(character);
            pos = new Vector3(pos.x - prefab[rand].GetComponentInChildren<Renderer>().bounds.size.x, pos.y);
        }
        gap = new Vector3(this[queueLength-1].GetComponentInChildren<Renderer>().bounds.size.x, 0);
    }

    public GameObject this[int i]
    {
        get
        {
            return list[i];
        }
    }

    void Update()
    {
    }

    public Vector3 GetPlaceForPlayer()
    {
        return pos;
    }

    public void ErasePerson(int i)
    {
        if (i == 0)
        {
            if(queueLength == 1)
                GameManager.eventSystem.Call("EmptyPlace", GameManager.eventSystem);

            if(i != queueLength-1)
            gap = new Vector3(Mathf.Abs(this[i + 1].transform.position.x - transform.position.x), 0);
            else
            {
                gap = new Vector3(1,0,0);
            }
        }
        else
        {
            Vector3 t = this[i + 1].transform.position - this[i - 1].transform.position;
            gap = new Vector3(Mathf.Abs(t.x) - this[i - 1].GetComponentInChildren<Renderer>().bounds.size.x, 0);
        }
        
        StartCoroutine("Shift", i + 1);
    }

    public void AddPerson(GameObject character)
    {
        Vector3 spawnPosition;
        if (queueLength == 0)
            spawnPosition = GameManager.player.GetNextPlacePosition();
        else
            spawnPosition = new Vector3(list[list.Count - 1].transform.position.x - list[list.Count - 1].GetComponentInChildren<Renderer>().bounds.size.x, transform.position.y);
        character.transform.parent = transform;
        list.Add(character);
        queueLength = list.Count;
    }

    public void AddPerson()
    {
        Vector3 spawnPosition;
        if (queueLength == 0)
            spawnPosition = GameManager.player.GetNextPlacePosition();
        else
            spawnPosition = new Vector3(list[list.Count - 1].transform.position.x - list[list.Count - 1].GetComponentInChildren<Renderer>().bounds.size.x, transform.position.y);


        list.Add(Instantiate(prefab[GameManager.instance.RandomRange(0,prefab.Length)],
            spawnPosition,
            Quaternion.identity, transform));
        queueLength = list.Count;
    }

    public void AddPerson(int num)
    {
        Vector3 spawnPosition;
        if (queueLength == 0)
            spawnPosition = GameManager.player.GetNextPlacePosition();
        else
            spawnPosition = new Vector3(list[list.Count - 1].transform.position.x - list[list.Count - 1].GetComponentInChildren<Renderer>().bounds.size.x, transform.position.y);

        list.Add(Instantiate(prefab[num],
            spawnPosition,
            Quaternion.identity, transform));
        queueLength = list.Count;
    }

    IEnumerator Shift(int i)
    {
        GetComponent<AudioSource>().Play();
        if (i == queueLength)
        {

            list.Remove(list[list.Count - 1]);
            queueLength = list.Count;
            yield break;
        }
        else
        {
            if (i == queueLength - 1)
            {
                GameManager.eventSystem.Call("EmptyPlace", GameManager.eventSystem);
            }
            float time = 0f;

            Vector3 newPos = this[i].transform.position + gap;
            
            while (true)
            {
                time += Time.deltaTime*movementSpeed;
                this[i].transform.position = Vector3.Lerp(newPos-gap, newPos, time);
                this[i].GetComponent<Animator>().SetBool("Running", true);
                if (this[i].transform.position == newPos)
                {
                    this[i].GetComponent<Animator>().SetBool("Running", false);
                    Debug.Log("Przesunięta osoba numer "+ i );
                    list[i - 1] = list[i];
                    StartCoroutine("Shift", ++i);
                    yield break;
                }

                yield return null;
            }
        }
    }
}
