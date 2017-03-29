using UnityEngine;
using System.Collections;

public class de_fireRate : AnyBuff {

    // Use this for initialization

    

    Weapon h,z;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        time = 15f;
        opis = "Decrease shooting speed!";
        isDebuff = true;
    }
	override protected void Affect(Collider col)
    {
        buffing = col.gameObject;
        Debug.Log(opis);
        h = buffing.GetComponent<PlayerMovement>().getWeapon();
        h.reload_weapon_timer *= 3f;
        buffing.GetComponent<PlayerMovement>().makeWeapon(h);
    }

    override protected void Deffect()
    {
        
        z = buffing.GetComponent<PlayerMovement>().getWeapon();
        if (h == z)
        {
            h.reload_weapon_timer /= 3f;
            buffing.GetComponent<PlayerMovement>().makeWeapon(h);
        }
        Debug.Log(opis + "off");
    }
}
