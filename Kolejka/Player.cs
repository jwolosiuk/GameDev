using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int placeAtBeggin = 0;

    public float waitTime = 2f;
    public float MOVEMENT_SPEED = 1f;
    public float BACKING_SPEED = 0.5f;

    public float movementTime = 0f;

    public float RESTING_TIME = 0.2f;

    public bool canMove = false;

    public float lastMovementTime;

    float epsilon = 0.1f;

    public Vector2 bumpVelocity = new Vector2(-100f, 0);

    Vector3 actualPlacePosition;
    int actualPlaceNumber;
    Vector3 nextPlacePosition;
    Vector3 startPosition;
    Vector3 endPosition;


    Animator animator;
    GameObject character;

    public enum States { Moving, Standing, Resting, Rest, Bump, GoingBack };
    public States state;

    Rigidbody2D rigid2D;

    public GameObject[] possibleCharacters;

    public void Init()
    {
        actualPlaceNumber = GameManager.instance.GetNumberOfPlaces()-1;
        character = Instantiate(possibleCharacters[GameManager.instance.RandomRange(0, possibleCharacters.Length)],Vector3.zero,Quaternion.identity,transform);
        character = GameManager.instance.RandomizeCharacterParameters(character);
        PlacePlayerAtEnd();
        state = States.Standing;
        Destroy(character.GetComponent<Rigidbody2D>());
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
        animator = character.GetComponent<Animator>();
        GameManager.eventSystem.Subscribe("GameEnds", LastBreathe);
    }

    void Update()
    {
        switch (state)
        {
            case States.Moving:
                if (movementTime*MOVEMENT_SPEED <= 1)
                {
                    movementTime += Time.deltaTime;
                    transform.position = Vector3.Lerp(actualPlacePosition, nextPlacePosition, movementTime * MOVEMENT_SPEED);
                }
                else
                {
                    if(canMove==true)
                    {
                        transform.position = nextPlacePosition;
                        actualPlacePosition = transform.position;
                        actualPlaceNumber--;
                        nextPlacePosition += GameManager.queue.gap;
                        movementTime = 0;
                        state = States.Rest;
                        GameManager.eventSystem.Call("Succeeded", GameManager.eventSystem);
                    }
                    
                }
                break;
            case States.Rest:
                state = States.Resting;
                animator.SetBool("Running", false);
                Invoke("EndRest", RESTING_TIME);
                break;
            case States.Standing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (GameManager.queue.queueLength == 0)
                        GameManager.eventSystem.Call("GameEnds", GameManager.eventSystem);
                    else 
                        MoveToNextPlace();
                }
                break;
            case States.Bump:
                if(rigid2D.velocity.magnitude<epsilon)
                {
                    state = States.GoingBack;
                    startPosition = transform.position;
                    endPosition = actualPlacePosition;
                }
                break;
            case States.GoingBack:
                animator.SetBool("Running", true);
                if (movementTime*BACKING_SPEED<=1)
                {
                    movementTime += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPosition, endPosition, movementTime * BACKING_SPEED);
                } else
                {
                    movementTime = 0;
                    transform.position = endPosition;
                    startPosition = transform.position;
                    endPosition += GameManager.queue.gap;
                    
                    state = States.Rest;
                }
                break;
        }
    }

    public int GetActualPlaceNumber()
    {
        return actualPlaceNumber;
    }

    public Vector3 GetNextPlacePosition()
    {
        return transform.position + GameManager.queue.gap;
    }

    void MoveToNextPlace()
    {
        actualPlacePosition = transform.position;
        nextPlacePosition = actualPlacePosition + GameManager.queue.gap;
        state = States.Moving;
        animator.SetBool("Running",true);
        movementTime = 0;
    }

    public void AllowMovementForTime(float time)
    {
        canMove = true;
        placeAtBeggin = actualPlaceNumber;
        Invoke("DisallowMovement", time);
    }

    void DisallowMovement()
    {

        if (placeAtBeggin == actualPlaceNumber)
        {
            GameManager.eventSystem.Call("Failed", GameManager.eventSystem);

        }
        canMove = false;
    }

    void EndRest()
    {
        state = States.Standing;
    }

    void PlacePlayerAtEnd()
    {
        transform.position = GameManager.queue.GetPlaceForPlayer();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == States.Moving)
        {
            Debug.Log("PANIE CO PAN");
            state = States.Bump;
            movementTime = 0;
            animator.SetBool("Running", false);
            GameManager.cameraManager.AddShake(20);
            endPosition = actualPlacePosition;
            startPosition = transform.position;
            rigid2D.velocity = bumpVelocity;
        }
        
    }

    void LastBreathe(EventInfoS e)
    {
        MovePlayer mover = gameObject.AddComponent<MovePlayer>();
        mover.animationFrames = new MovePlayer.SimpleAniStep[2];
        mover.animationFrames[0] = new MovePlayer.SimpleAniStep(new Vector3(5.7f, -1f, 0), 1, 2, 0);
        mover.animationFrames[1] = new MovePlayer.SimpleAniStep(new Vector3(5.7f, -0.8f, 0), 0, 1f, 0);

        mover.destroyAtTheEnd = true;
        mover.Play();
    }

    private void OnDestroy()
    {
        GameManager.instance.LoadScene(2);
    }
}
