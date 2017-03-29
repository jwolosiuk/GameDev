using UnityEngine;
using System.Collections;

public class de_walkingSpeed : AnyBuff {

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        time = 6f;
        opis = "Decrease walking speed!";
    }
	override protected void Affect(Collider col)
    {
        buffing = col.gameObject;
        Debug.Log("walkingdeBuff");
        buffing.GetComponent<PlayerMovement>().walkingSpeed /= 2f;
    }

    override protected void Deffect()
    {
        buffing.GetComponent<PlayerMovement>().walkingSpeed *= 2.5f;
        Debug.Log("turn off walkingdeBuff");
    }
}
