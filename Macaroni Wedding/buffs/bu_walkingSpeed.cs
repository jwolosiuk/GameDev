using UnityEngine;
using System.Collections;

public class bu_walkingSpeed : AnyBuff {

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        time = 10f;
        opis = "Increase walking speed!";
    }
	override protected void Affect(Collider col)
    {
        buffing = col.gameObject;
        Debug.Log("walkingBuff");
        buffing.GetComponent<PlayerMovement>().walkingSpeed *= 1.2f;
    }

    override protected void Deffect()
    {
        buffing.GetComponent<PlayerMovement>().walkingSpeed /= 1.2f;
        Debug.Log("turn off walkingBuff");
    }
}
