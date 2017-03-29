using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {


    Text scoreText;

    public Text text { get { return scoreText; } set { scoreText = value; } }

    void Awake()
    {
        scoreText = GetComponentInChildren<Text>();
    }

}
