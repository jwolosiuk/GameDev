using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public struct AudioPair
    {
        public string name;
        public bool repeatable;
        public float reapeatTime;
        public AudioClip audio;
    }

    public AudioPair[] audios;
    float[] lastTimePlayed;
    AudioSource audioSource;

    public static GameManager instance;
    public static IgorEventSystemS eventSystem;
    public static Player player;
    public static Queue queue;
    public static OccurenceManager occurenceManager;
    public static CameraManager cameraManager;
    public static GameObject mainCamera;


    public float MIN_IMPATIENCE = 0.3f;
    public float MAX_IMPATIENCE = 1.7f;
    public int turn = 0;
    public int playerPosition;

    System.Random randomSalt = new System.Random();
    //References
    public GameObject queueReference;
    public GameObject occurenceReference;
    public GameObject playerReference;

    enum States { NotInitialized, GameManagerReady, GameReady ,GameRunning, GameOver, NumOfStates};
    States state;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        Init();
    }

    void Init()
    {
        mainCamera = Camera.main.gameObject;
        cameraManager = mainCamera.GetComponent<CameraManager>();
        state = States.NotInitialized;
        eventSystem = GetComponent<IgorEventSystemS>();
        queue = queueReference.GetComponent<Queue>();
        occurenceManager = occurenceReference.GetComponent<OccurenceManager>();
        player = playerReference.GetComponent<Player>();
        foreach (string s in eventSystem.eventsList)
            eventSystem.Subscribe(s, DefaultEvent);


        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        if (!audioSource.isPlaying)
            Debug.Log("Not playing");
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 0.5f;

        lastTimePlayed = new float[audios.Length];
        for (int i = 0; i < lastTimePlayed.Length; i++)
            lastTimePlayed[i] = Time.time;


        occurenceManager.Init();
        queue.Init();
        player.Init();

        state = States.GameManagerReady;
    }

    private void Update() // Write later
    {
        switch (state)
        {
            case States.GameManagerReady:
                state = States.GameRunning;
                eventSystem.Call("Start", eventSystem);
                break;
            case States.GameRunning:
                for (int i = 0; i < audios.Length; ++i)
                    if (audios[i].repeatable && Time.time - lastTimePlayed[i] > audios[i].reapeatTime)
                    {
                        lastTimePlayed[i] = Time.time;
                        audioSource.PlayOneShot(audios[i].audio);
                    }
                break;
        }
    }

    void DefaultEvent(EventInfoS e)
    {
        Debug.Log("Event sie dzieje: " + eventSystem.eventsList[e.eventId]);
    }

    public int GetNumberOfPlaces()
    {
        return occurenceManager.gOCycles.Length;
    }

    public GameObject RandomizeCharacterParameters(GameObject character)
    {
        Animator animator = character.GetComponent<Animator>();
        animator.SetFloat("Impatience", RandomRange(0.3f, 1.5f));
        animator.SetFloat("Offset", RandomRange(0f,1f));

        return character;
    }

    public int RandomRange(int min, int max)
    {
        return randomSalt.Next(min, max); ;
    }

    public float RandomRange(float min, float max)
    {
        double val = randomSalt.NextDouble(); // range 0.0 to 1.0
        val *= max - min;
        val += min;
        return (float)val;
    }

    public void PlaySound(string name, float volume = 1.0f)
    {
        Debug.Log("Playing a sound");
        int index = Array.FindIndex(audios, s => name == s.name);
        audioSource.PlayOneShot(audios[index].audio, volume);
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

}

