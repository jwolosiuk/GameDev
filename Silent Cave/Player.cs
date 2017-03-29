using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //SET TRIGERS LATER!!!      

    [System.Serializable]
    public struct AudioPair
    {
        public string name;
        public AudioClip audio;
    }

    public float slideLenght;
    public float dashLenght;
    public float timeBetweenSteps;
    public float offsetZ;
    public GameObject bullet;
    public GameObject instantiatedBullet;
    public GameObject leftovers;

    bool controlAllowed;

    Animator animatorController;
    Rigidbody rigidBody;

    public AudioPair[] audios;
    AudioSource audioSource;
    float lastTimePlayedSteps;
    float slideStartTime;
    float dashStartTime;

    CapsuleCollider normalCollider;
    BoxCollider slideCollider;
    ParticleSystem particleEffect;

    public enum States {Run, Jump, Slide, DoubleJump, Dash, Attack, Dead, CompletedJump, CompleteDash, DashJump, SlideJump}
    public States state;


	void Start () {
        controlAllowed = false;
        //animatorController = GetComponent<Animator>();
        animatorController = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        normalCollider = GetComponent<CapsuleCollider>();
        slideCollider = GetComponent<BoxCollider>();
        particleEffect = GetComponent<ParticleSystem>();
        slideCollider.enabled = false;

        lastTimePlayedSteps = 0f;

        GameManager.eventSystem.Subscribe("GameInitialized", AllowControl);
        GameManager.eventSystem.Subscribe("PlayerDied", Die);

        instantiatedBullet = null;
        state = States.Run;

    }
	
	void Update () {
        if (controlAllowed)
            ExecuteInput(); 

	}

    void AllowControl(EventInfoS e)
    {
        controlAllowed = true;
    }   

    void ExecuteInput()
    {
        if (GameManager.playerControl.CanSlide())
                ExecuteDown();
        else if (GameManager.playerControl.CanDouble())
                ExecuteUp();
        else
                ExecuteDefault();  
        
        if(state == States.Run)
            if(GameManager.instance.gameTime - lastTimePlayedSteps > timeBetweenSteps)
            {
                PlaySound("Run");
                lastTimePlayedSteps = GameManager.instance.gameTime;
            }

        if(state != States.Dead)
            gameObject.transform.position += new Vector3(GameManager.instance.gameSpeed * Time.deltaTime, 0, 0);
    }

    void ExecuteDown()
    {
        switch (state)
        {
            case States.Dash:
                if (GameManager.instance.Scream())
                    NoExcAttack();
                Dash();
                break;
            case States.Jump:
                if (GameManager.instance.Scream())
                    DoubleJump(true);
                break;
            case States.Run:
            case States.CompletedJump:
            case States.CompleteDash:
            case States.Attack:
                Crouch(true);
                break;
            case States.Slide:
                if (GameManager.instance.Scream())
                    Dash(true);
                else
                    Crouch();
                break;
            case States.DashJump:
                if (GameManager.instance.Scream())
                {
                    normalCollider.enabled = true;
                    slideCollider.enabled = false;
                    GameManager.instance.gameSpeed -= GameManager.instance.dashGameSpeed;
                    DoubleJump(true);
                }
                break;
            case States.SlideJump:
                if (GameManager.instance.Scream())
                {
                    normalCollider.enabled = true;
                    slideCollider.enabled = false;
                    DoubleJump(true);
                }
                break;
        }

    }
    void ExecuteUp()
    {
        switch (state)
        {
            case States.Run:
            case States.CompletedJump:
            case States.CompleteDash:
            case States.Attack:
                Jump();
                break;
            case States.Jump:
                if (GameManager.instance.Scream())
                    DoubleJump(true);
                break;
            case States.SlideJump:
                if (GameManager.instance.Scream())
                {
                    normalCollider.enabled = true;
                    slideCollider.enabled = false;
                    DoubleJump(true);
                }
                break;
            case States.Slide:
                SlideJump();
                break;
            case States.DashJump:
                if (GameManager.instance.Scream())
                {
                    normalCollider.enabled = true;
                    slideCollider.enabled = false;
                    GameManager.instance.gameSpeed -= GameManager.instance.dashGameSpeed;
                    DoubleJump(true);
                }
                break;
            case States.Dash:
                DashJump(true);
                break;
        


        }
    }
    void ExecuteDefault()
    {

        switch (state)
        {
            case States.CompletedJump:
            case States.CompleteDash:
            case States.Attack:
                Run();
                break;
            case States.Run:
                if (GameManager.instance.Scream())
                    Attack(true);
                break;
            case States.Jump:
                if (GameManager.instance.Scream())
                    DoubleJump(true);
                break;
            case States.Slide:
                if (GameManager.instance.Scream())
                    Dash(true);
                else
                    Crouch();
                break;
            case States.Dash:
                if (GameManager.instance.Scream())
                    NoExcAttack();
                Dash();
                break;
            case States.DashJump:
                if (GameManager.instance.Scream())
                { 
                    normalCollider.enabled = true;
                    slideCollider.enabled = false;
                    GameManager.instance.gameSpeed -= GameManager.instance.dashGameSpeed;
                    DoubleJump(true);
                }
                break;
            case States.SlideJump:
                if (GameManager.instance.Scream())
                { 
                    normalCollider.enabled = true;
                    slideCollider.enabled = false;
                    DoubleJump(true);
                }
                break;
            }
        
    }


    void Run()
    {
        lastTimePlayedSteps = 0;
        state = States.Run;
        animatorController.SetTrigger("Run");
    } 
    void Attack(bool start = false)
    {
            //state = States.Attack;
            animatorController.SetTrigger("Attack");
            //GameManager.instance.PlaySound("Crystals");
            if(instantiatedBullet == null)
                instantiatedBullet = Instantiate(bullet, gameObject.transform.position + new Vector3(0,0,offsetZ), Quaternion.identity);       
    }

    void NoExcAttack()
    {
        if (instantiatedBullet == null)
            instantiatedBullet = Instantiate(bullet, gameObject.transform.position + new Vector3(0, 0, offsetZ), Quaternion.identity);
    }

    void Jump()
    {
        PlaySound("Jump");
        state = States.Jump;
        //rigidBody.AddForce(transform.up*GameManager.instance.jumpHigh);
        rigidBody.velocity = new Vector3(0, GameManager.instance.jumpHigh, 0);
        animatorController.SetTrigger("Jump");
    }
    void DoubleJump(bool start = false)
    {
        if (start)
        {
            PlaySound("Jump");
            GameManager.instance.PlaySound("Crystals");
            state = States.DoubleJump;
            //rigidBody.AddForce(transform.up * GameManager.instance.jumpHigh);
            rigidBody.velocity = new Vector3(0, GameManager.instance.jumpHigh, 0);
            animatorController.SetTrigger("DoubleJump");
            particleEffect.Play();
        }
    }
    void CompletedJump()
    {
        if(state == States.SlideJump || state == States.DashJump)
        {
            normalCollider.enabled = true;
            slideCollider.enabled = false;
        }
        if (state == States.DashJump)
            GameManager.instance.gameSpeed -= GameManager.instance.dashGameSpeed;

        state = States.CompletedJump;
        //animation trigger
    }
    void Crouch(bool start = false)
    {
        if (start)
        {
            state = States.Slide;
            animatorController.SetTrigger("Slide");
            //PlaySound("Dash");
            slideStartTime = GameManager.instance.gameTime;
            slideCollider.enabled = true;
            normalCollider.enabled = false;
        }
        else
        {
            if (GameManager.instance.gameTime - slideStartTime > slideLenght)
            {
                Run();
                normalCollider.enabled = true;
                slideCollider.enabled = false;
            }
        }
    }
    void Dash(bool start = false)
    {
        if(start)
        {
            state = States.Dash;
            animatorController.SetTrigger("Dash");
            PlaySound("Dash");
            dashStartTime = GameManager.instance.gameTime;
            slideCollider.enabled = true;
            normalCollider.enabled = false;
            particleEffect.Play();

            GameManager.instance.gameSpeed += GameManager.instance.dashGameSpeed;
        }
        else
        {
            if (GameManager.instance.gameTime - dashStartTime > dashLenght)
            {
                state = States.CompleteDash;
                normalCollider.enabled = true;
                slideCollider.enabled = false;

                GameManager.instance.gameSpeed -= GameManager.instance.dashGameSpeed;
            }
        }
    }

    void DashJump(bool start = false)
    {
        if (start)
        {
            state = States.DashJump;
            PlaySound("Jump");
            rigidBody.velocity = new Vector3(0, GameManager.instance.jumpHigh, 0);
            slideCollider.enabled = true;
            normalCollider.enabled = false;
            //particleEffect.Play();
        }
    }
    void SlideJump()
    {
        PlaySound("Jump");
        rigidBody.velocity = new Vector3(0, GameManager.instance.jumpHigh, 0);
        state = States.SlideJump;
    }

    void Die(EventInfoS s)
    {
        Debug.Log("I'm Dying");
        state = States.Dead;
        controlAllowed = false;

        GameObject corpses = Instantiate(leftovers, transform.position, transform.rotation) as GameObject;
        Destroy(corpses, 5f);
        Destroy(gameObject);
                   
           
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            switch (state)
            {
                case States.Jump:
                case States.DoubleJump:
                case States.DashJump:
                case States.SlideJump:
                    CompletedJump();
                    break;
            }
        }
    }

    public void PlaySound(string name, float volume = 1.0f)
    {
        int index = Array.FindIndex(audios, s => name == s.name);
        audioSource.PlayOneShot(audios[index].audio, volume);
    }
}
