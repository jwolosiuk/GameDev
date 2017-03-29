using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public string AnimatorTag; 

    Vector3 initialPos;
    Animator animator;
    // Use this for initialization
    void Start()
    {
        //initialPos = transform.position;
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        animator.SetBool(AnimatorTag, true);
        //StopCoroutine("Move");
        //StartCoroutine("Move", new Vector3(0,-10,1));
    }
    void OnMouseExit()
    {
        animator.SetBool(AnimatorTag, false);
        //StopCoroutine("Move");
        //StartCoroutine("Move", new Vector3(0,0,1));
    }

    public void MoveCoroutine(Vector3 vec)
    {
        StartCoroutine("Move", vec);
    }

    IEnumerator Move(Vector3 vec)
    {
        float t = 0f;

        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, initialPos + vec, t);
            t += Time.deltaTime*vec.z;

            if(transform.position == vec)
            {
                yield break;
            }

            yield return null;
        }
    }
}
