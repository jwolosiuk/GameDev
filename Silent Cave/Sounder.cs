using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounder : MonoBehaviour {

    AudioClip audio;
    public float sensitivity = 1;
    [Range(0, 44100)]
    public int frequency;
    public int audioLenght;
    public int dataTableSize;

    bool microReady;

    public bool isRecording { get { return Microphone.IsRecording(null); } }
    public bool isMicro { get { return microReady; } }

    void Start()
    {
       
       if(!(Microphone.devices.Length > 0))
        {
            Debug.Log("No Microphones found!");
            microReady = false;
        }
        else
            microReady = true; //if problems with device set it here 

       if(microReady)
        {
            audio = Microphone.Start(null, true, audioLenght, frequency);
        }

    } 

    public float[] GetLastSound(int soundLength)
    {
        float[] data;
        int tableSize;
        int offset = 0;
        tableSize = soundLength;
        offset = Microphone.GetPosition(null) - tableSize;
        if (offset < 0)
        {
            tableSize = Microphone.GetPosition(null);
            offset = 0;
        }
        if (tableSize < 1)
            tableSize = soundLength;
        data = new float[tableSize];
        audio.GetData(data, offset);
        return data;
    }

    public float GetAveragedVolume()
    {
        float volume = 0;
        int tableSize = dataTableSize;
        float[] data = new float[tableSize];
        data = GetLastSound(tableSize);

        foreach (float s in data)
            volume += Mathf.Abs(s);

        volume *= sensitivity;

        return volume / tableSize;
    }



}
