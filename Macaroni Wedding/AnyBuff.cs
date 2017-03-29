using UnityEngine;
using System.Collections;

public class AnyBuff : MonoBehaviour
{
    protected float ActivationTime = 0.2f;
    protected float time = 10f;
    protected string opis = "";
    [SerializeField]
    protected float Speed = 0.3f;
    [SerializeField]
    float JumpMultiplier = 6f;

    protected bool isDebuff = false;

    GameObject catched;
    GameObject puff;

    const float minHeight = 0.2f;
    protected GameObject buffing = null;
    bool Active;
    Transform Target;

    protected Rigidbody rigid;
    //[SerializeField]
    protected AudioClip AffectSound;
    //AudioSource audio = GameObject.Find("audio").GetComponent<AudioSource>();


    void Awake()
    {
        catched = (GameObject)Resources.Load("BoostEffect");
        puff = (GameObject)Resources.Load("BuffExplosion");
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(Active && Target)
        {
            Vector3 move = Target.position - transform.position;
            move -= Vector3.up*move.y;
            move.Normalize();
            move *= Speed;
            rigid.MovePosition(transform.position + move);

            if (transform.position.y < minHeight)
            {
                //transform.position -= Vector3.up * transform.position.y;
                //transform.position += Vector3.up * minHeight;
                JumpToTarget();
            }
        }
            

            
	}

    void OnColliderEnter(Collision col)
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(!Active)
        if (col.gameObject.tag == "male" || col.gameObject.tag == "female")
        {
            Activate();

            if (col.gameObject.tag == "male")
                Target = GameObject.FindGameObjectWithTag("female").transform;

            if (col.gameObject.tag == "female")
                Target = GameObject.FindGameObjectWithTag("male").transform;

            //JumpToTarget();
        }
        if(Target != null)
        if (col.gameObject.tag == Target.tag)
        {
            Affect(col);
                //if(AffectSound != null)
                //     audio.PlayOneShot(AffectSound);

            GameObject tmp= null;                                       //Utworzenie puffnięcia przy dotknięciu
            if(puff != null)
                tmp = (GameObject)Instantiate(puff, transform.position+Vector3.up, Quaternion.identity); 
            Destroy(tmp, 2f);
                       
            if (catched != null)                                       //Utworzenie iskierek sygnalizujących uruchomionego buffa
               tmp = (GameObject)Instantiate(catched, Target.position, Quaternion.identity);
            tmp.transform.parent = Target;
            Destroy(tmp, time);

            Invoke("Deffect", time);
            gameObject.SetActive(false);
            DestroyBuff(time);
        }
    }

    protected void DestroyBuff(float x)
    {
        Destroy(gameObject, x+0.3f);
    }

    protected void Activate()
    {
        Active = true;
        GetComponent<Collider>().isTrigger = true;
    }

    protected void JumpToTarget()
    {
        Vector3 move = Target.position - transform.position;
        move = Vector3.up * move.magnitude * JumpMultiplier;
        rigid.AddForce(move);
    }

    virtual protected void Affect(Collider col)
    {
        Debug.Log("Affect. To powinno byc nadpisane");
    }

    virtual protected void Deffect()
    {
        Debug.Log("Deffect. To tez powinno byc nadpisane");
    }
}
