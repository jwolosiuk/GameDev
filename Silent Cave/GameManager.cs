using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public struct AudioPair
    {
        public string name;
        public AudioClip audio; 
    }

    public float pointsMultiplier;
    public float SoundTreshold;
    public float gameSpeed;
    public float jumpHigh;
    public float timeBetweenDroplets;
    public float dashGameSpeed;
    public float inBetweenScreamTime;

    public static GameManager instance;
    public static Sounder mic;
    public static IgorEventSystemS eventSystem;
    public static IControl playerControl;
    public static Player player;

    public AudioPair[] audios;
    AudioSource audioSource;
    EndScreen endScreen;

    float time;
    float lastTimePlayedDroplets;
    float lastTimeScreamed;
    int points;
    Text pointsText;

    bool _newSound;
    public bool newSound { get { return _newSound; } }

    enum States { NotInitialized ,GameInitialized, GameRunning, GameOver };
    States state;

    public float soundTreshold { get { return SoundTreshold; } }
    public float gameTime { get { return time; } }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
        Init(); // To init all of the shit 

    }

    private void Update()
    {
        time += Time.deltaTime;
        
        switch (state)
        {
            case States.GameInitialized:
                eventSystem.Subscribe("PlayerDied", PlayerDied);
                eventSystem.Call("GameInitialized", eventSystem);
                state = States.GameRunning;
                break;
            case States.GameRunning:
                gameSpeed += Time.deltaTime / 13;
                if (time - lastTimePlayedDroplets > timeBetweenDroplets)
                {
                    PlaySound("Droplets", 2f);
                    lastTimePlayedDroplets = time;   
                }
                points += (int)(player.transform.position.x * pointsMultiplier);
                pointsText.text = points.ToString();
                break;

        }
    }

    void Init() 
    {
        state = States.NotInitialized;

        mic = GetComponent<Sounder>();
        eventSystem = GetComponent<IgorEventSystemS>();

        _newSound = true;

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        if (!audioSource.isPlaying)
            Debug.Log("Not playing");
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 0.5f;

        string[] controllers = Input.GetJoystickNames();
        if (controllers.Length > 0)
        {
            playerControl = new Pad();
            Debug.Log("Pad connected!");
        }
        else
        { 
            playerControl = new Keyboard();
            Debug.Log("Keybard Connected");
        }

        lastTimePlayedDroplets = 0f;
        player = GameObject.Find("Player").GetComponent<Player>();
        points = 0;
        Text[] tempTab = GetComponentsInChildren<Text>();
        foreach (Text t in tempTab)
            if (t.gameObject.name == "Score")
                pointsText = t;
        pointsText.text = "0";
        endScreen = GetComponentInChildren<EndScreen>();
        endScreen.gameObject.SetActive(false);
        state = States.GameInitialized; 
        

    }

    public bool ReachedSoundTreshold()
    {
        return SoundTreshold < mic.GetAveragedVolume();
    }
    public bool Scream()
    {
        if(gameTime - lastTimeScreamed > inBetweenScreamTime && ReachedSoundTreshold())
        {
            lastTimeScreamed = gameTime;
            return true;
        }
        return false;
    }

    public void PlaySound(string name, float volume = 1.0f)
    {
        int index = Array.FindIndex(audios, s => name == s.name);
        audioSource.PlayOneShot(audios[index].audio, volume);
    }

    void PlayerDied(EventInfoS e)
    {
        state = States.GameOver;
        endScreen.gameObject.SetActive(true);
        PlaySound("Dead");
        endScreen.text.text = "Your score:\n" + points.ToString();
        pointsText.gameObject.SetActive(false);
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id); 
    }

}
