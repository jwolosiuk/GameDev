using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public enum States { NotPlaying, Playing, Die, numOfStates };
    States state = States.NotPlaying;

    public States getState { get { return state; } }

    [System.Serializable]
    public struct SimpleAniStep
    {
        public Vector3 targetPosition;
        public float targetAlpha;
        public float stepConstSpeed;
        public float stepPercentegSpeed;

        public SimpleAniStep(Vector3 position, float alpha, float stepConst, float stepPercentege)
        {
            targetAlpha = alpha;
            targetPosition = position;
            stepConstSpeed = stepConst;
            stepPercentegSpeed = stepPercentege;
        }
    }

    public SimpleAniStep[] animationFrames;
    public bool destroyAtTheEnd;
    public bool debug;

    SpriteRenderer[] spriteRenderers;

    Animator animator;

    int frame;
    float currentAlpha;

    private void Awake()
    {
        currentAlpha = 0;
        state = States.NotPlaying;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (debug)
            Play();
    }

    void Update () {
	    if(state == States.Playing )
        {
            float stepConst = animationFrames[frame].stepConstSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, animationFrames[frame].targetPosition, stepConst);

            float stepPercentage = animationFrames[frame].stepPercentegSpeed * Time.deltaTime * Vector3.Distance(animationFrames[frame].targetPosition, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, animationFrames[frame].targetPosition, stepPercentage);

            if (animator != null)
            {
                if (transform.position == animationFrames[frame].targetPosition)
                    animator.SetBool("Running", false);
                else
                    animator.SetBool("Running", true);
            }

            foreach(SpriteRenderer s in spriteRenderers)
            {
                stepConst = animationFrames[frame].stepConstSpeed * Time.deltaTime;
                Color colorTemp = s.color;
                if ((colorTemp.a -= stepConst) < animationFrames[frame].targetAlpha)
                    colorTemp.a = animationFrames[frame].targetAlpha;


                s.color = colorTemp;
                

                stepPercentage = animationFrames[frame].stepPercentegSpeed * Time.deltaTime;
                colorTemp = s.color;
                                
                if ((colorTemp.a -= Mathf.Abs(colorTemp.a - animationFrames[frame].targetAlpha) * stepPercentage) < animationFrames[frame].targetAlpha)
                    colorTemp.a = animationFrames[frame].targetAlpha;

                s.color = colorTemp;

                currentAlpha = s.color.a;
            }
            if (transform.position == animationFrames[frame].targetPosition && currentAlpha == animationFrames[frame].targetAlpha)
            {
                frame++;
                if (frame > animationFrames.Length - 1)
                    state = States.Die;
            }

        }
        if (state == States.Die)
            if (destroyAtTheEnd)
                Destroy(gameObject);
	}   

    public void Play()
    {
        frame = 0;
        state = States.Playing;
    }
}
