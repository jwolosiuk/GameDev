using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    Animator animator;
    bool isInput;

    void Start()
    {
        isInput = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isInput)
        {
            animator.SetTrigger("Start");
            Invoke("Shake", 1);
            GetComponent<AudioSource>().Play();
        }
    }

    void Shake()
    {
        Camera mainCam = Camera.main;
        mainCam.GetComponent<CameraManager>().AddShake(100);
        Invoke("GoToMainScene", 1);
    }

    void GoToMainScene()
    {
        SceneManager.LoadScene(1);
        DontDestroyOnLoad(this);
        animator.SetTrigger("Reveal");
        isInput = false;
    }
}
