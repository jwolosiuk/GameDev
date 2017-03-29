using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpawner : MonoBehaviour {
    Vector3 leftCarPosition, rightCarPosition, leftPasserbyPosition, rightPasserbyPosition;
    float passTime = 0;
    float carTime = 0;

    public GameObject[] cars;
    public GameObject[] carsReversed;

    public float MIN_PASS_SPEED;
    public float MAX_PASS_SPEED;
    public float MIN_PASS_DELAY;
    public float MAX_PASS_DELAY;

    public float MIN_CAR_SPEED;
    public float MAX_CAR_SPEED;
    public float MIN_CAR_DELAY;
    public float MAX_CAR_DELAY;

    float carDelay = 0;
    float passDelay = 0;

    private void Start()
    {
        leftCarPosition = new Vector3(-8, -3.3f, 0);
        rightCarPosition = new Vector3(12, -3.3f, 0);
        leftPasserbyPosition = new Vector3(-8, -1.2f, 0);
        rightPasserbyPosition = new Vector3(8, -1.2f, 0);
    }

    private void Update()
    {
        if (Time.time - passTime> passDelay)
        {
            passDelay = GameManager.instance.RandomRange(MIN_PASS_DELAY, MAX_PASS_DELAY);
            passTime = Time.time;
            SpawnPasserby();
        }
        if (Time.time - carTime > carDelay)
        {
            carDelay = GameManager.instance.RandomRange(MIN_CAR_DELAY, MAX_CAR_DELAY);
            carTime = Time.time;
            SpawnCar();
        }
    }

    void SpawnCar()
    {
        Vector3 start = Vector3.zero, end = Vector3.zero;
        int ktory = GameManager.instance.RandomRange(0, cars.Length+carsReversed.Length);
        bool flip = false;
        string name2 = "";
        int id2 = 0;
        GameObject next;
        start = leftCarPosition;
        end = rightCarPosition;
        if(ktory >= cars.Length)
        {
            flip = true;
            end = leftCarPosition+new Vector3(0,0.4f,0);
            start = rightCarPosition+ new Vector3(0, 0.4f, 0);
            ktory -= cars.Length;
            next = carsReversed[ktory];
            name2 = "Back Lane";
            id2 = 11;
        }
        else
        {
            name2 = "Front Lane";
            id2 = 12;
            next = cars[ktory];
        }
   
        
        GameObject walker = Instantiate(next, start, Quaternion.identity);

        MovePlayer mover = walker.AddComponent<MovePlayer>();

        LayerManager layerManager = walker.AddComponent<LayerManager>();
        layerManager.name2 = name2;
        layerManager.flipX = flip;
        layerManager.Init();
        layerManager.SetLayerRecursively(layerManager.gameObject, id2);
        mover.animationFrames = new MovePlayer.SimpleAniStep[1];
        float speed = GameManager.instance.RandomRange(MIN_CAR_SPEED, MAX_CAR_SPEED);
        mover.animationFrames[0] = new MovePlayer.SimpleAniStep((end-start)*3 + end, 1, speed, 0);
        mover.destroyAtTheEnd = true;
        mover.Play();
    }

    void SpawnPasserby()
    {
        Vector3 start=Vector3.zero, end=Vector3.zero;
        int ktory = GameManager.instance.RandomRange(0, 2);
        bool flip = false;
        string name2 = "";
        int id2 = 0;
        switch (ktory)
        {
            case 0:
                start = leftPasserbyPosition;
                end = rightPasserbyPosition;
                name2="PasserbyToRight";
                id2 = 9;
                break;
            case 1:
                flip = true;
                end = leftPasserbyPosition;
                start = rightPasserbyPosition;
                name2 = "PasserbyToLeft";
                id2 = 10;
                break;
        }

        int rand = GameManager.instance.RandomRange(0, GameManager.queue.prefab.Length);
        GameObject walker = Instantiate(GameManager.queue.prefab[rand], start, Quaternion.identity);

        MovePlayer mover = walker.AddComponent<MovePlayer>();

        LayerManager layerManager = walker.AddComponent<LayerManager>();
        layerManager.name2 = name2;
        layerManager.flipX = flip;
        layerManager.Init();
        layerManager.SetLayerRecursively(layerManager.gameObject, id2);
        walker.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        mover.animationFrames = new MovePlayer.SimpleAniStep[1];
        float speed = GameManager.instance.RandomRange(MIN_PASS_SPEED, MAX_PASS_SPEED);
        mover.animationFrames[0] = new MovePlayer.SimpleAniStep(end, 1, speed, 0);
        mover.destroyAtTheEnd = true;
        mover.Play();
    }
}
