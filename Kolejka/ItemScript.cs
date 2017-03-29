using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

    //public static int OverallCycles = 0;

    public static bool choosen = false;

    public int Threashold = 10;

    public Animator sceneAnimator;
    public SpriteRenderer popUp;

    Animator animator;

	// Use this for initialization
	void Awake ()
    {
        animator = GetComponent<Animator>();
	}

    void Update()
    {
        if (OccurenceManager.cyclesDone > Threashold)
            Destroy(gameObject);
    }

    void OnMouseEnter()
    {
        if (!choosen)
            animator.SetTrigger("Shake");
    }

    void OnMouseDown()
    {
        if(!choosen)
        {
            BeginShow();
            choosen = true;
        }
            
    }

    void BeginShow()
    {
        sceneAnimator.SetTrigger("Reveal");
        popUp.sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
