using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    float Damage = 20f;
    [SerializeField]
    float DeathDelay = 0.7f;

    [SerializeField]
    GameObject Corpse;
    GameManager gameManager = null;
    Animator ani;
    bool Alive = true;
    bool Dead = false;

    AudioSource audio;

    public bool IsAlive
    { get { return Alive; } }


	// Use this for initialization
	void Start ()
    {
        audio = GameObject.Find("audio").GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(!Alive && !Dead)
            Die();
	}

    public void TakeDamage(float DamageValue,float stun=0.1f)
    {
        Damage -= DamageValue;
        if(Alive)
        if (Damage <= 0f)
        {
            Damage = 0f;
            Alive = false;
        }
        else
        {
            if(ani != null)
                ani.SetTrigger("Hurt");
            gameObject.GetComponent<EnemyBehav>().Stun(stun);
            AudioClip pain= gameManager.GetComponent<randomizer>().GetPain();
                audio.PlayOneShot(pain);
            }
    }

    void Die()
    {
        ani.SetInteger("Orientation", 5);
        ani.SetTrigger("Died");
        AudioClip death = gameManager.GetComponent<randomizer>().GetDeath();
        audio.PlayOneShot(death);
        Destroy(GetComponent<EnemyBehav>());
        gameManager.EnemyDeath();
        Invoke("DestroyEnemy", DeathDelay);
        
        this.enabled = false;
        Dead = true;
    
    }

    void DestroyEnemy()
    {
        if (Corpse != null)
        {
            Instantiate(Corpse, transform.position,Quaternion.identity);
            Debug.Log("Leżą zwłoki!");
        }
            
        Destroy(gameObject);
    }
}
