using UnityEngine;
using System.Collections;

public class bu_fireRate : AnyBuff {

    Weapon h,z;
    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        time = 15f;
        opis = "Increase shooting speed!";
    }
	override protected void Affect(Collider col)
    {
        buffing = col.gameObject;
        Debug.Log(opis);
        h=buffing.GetComponent<PlayerMovement>().getWeapon();
        h.reload_weapon_timer /= 2f;
        buffing.GetComponent<PlayerMovement>().makeWeapon(h);
    }

    override protected void Deffect()
    {

        z = buffing.GetComponent<PlayerMovement>().getWeapon();
        if (h == z)
        {
            h.reload_weapon_timer *= 2f;
            buffing.GetComponent<PlayerMovement>().makeWeapon(h);
        }
        
        Debug.Log(opis + "off");
    }
}
