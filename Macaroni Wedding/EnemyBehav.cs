using UnityEngine;
using System.Collections;

public class EnemyBehav : MonoBehaviour
{

    public float walkingSpeed = 0.1f;
    public float pullingSpeed = 0.06f;
    public float slowingMultifier = 0.9f;
    public bool pull = false;

    Vector3 lastPosition;
    Animator ani;

    Vector3 diff = Vector3.zero;
    public GameObject target;
    public GameObject targetFiance;
    GameObject male;
    GameObject female;

    void Start()
    {
        ani = GetComponent<Animator>();
        if (gameObject.tag == "enemy_male")
        {
            target = GameObject.FindGameObjectWithTag("male");
            targetFiance = GameObject.FindGameObjectWithTag("female");
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("female");
            targetFiance = GameObject.FindGameObjectWithTag("male");
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == target )//|| (col.gameObject.tag == gameObject.tag && col.GetComponent<EnemyBehav>().pull == true))
        {
            SetPulling(true);
            diff = transform.position - target.transform.position;
        }
    }

    public void SetPulling(bool czy)
    {
        pull = czy;
        ani.SetBool("Pulling", czy);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == target)
        {
            SetPulling(false);
        }

    }

    public void Stun(float time)
    {
        SetPulling(false);
        this.enabled = false;
        Invoke("ChangeState", time);
    }

    void ChangeState()
    {
        this.enabled = true;
    }

    void OrientationFromDirection(Vector3 vec)
    {
        int val = 0;
        if (vec.x >= Mathf.Sqrt(2) / 2) val = 1;
        else
            if (vec.x <= -Mathf.Sqrt(2) / 2) val = 3;
        else
            if (vec.y >= Mathf.Sqrt(2) / 2) val = 0;
        else if (vec.y <= Mathf.Sqrt(2) / 2) val = 2;
        else
            val = 5;
        ani.SetInteger("Orientation", val);
    }

    void FixedUpdate()
    {
        if (pull)
        {
            Vector3 direction = target.transform.position - targetFiance.transform.position;
            target.GetComponent<PlayerMovement>().AddEnemy(direction.normalized * pullingSpeed, slowingMultifier);
            transform.position = target.transform.position + diff;
        }
        else
        {
            Vector3 nowy = target.transform.position - transform.position;
            nowy.Normalize();
            transform.Translate(nowy * walkingSpeed);
        }
        OrientationFromDirection(target.transform.position - transform.position);
    }
}
