using UnityEngine;
using System.Collections;

public class recoDecrease : AnyBuff {
    Weapon h,z;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        time = 15f;
        opis = "Decrease shooting recoil!";
    }
	override protected void Affect(Collider col)
    {
        buffing = col.gameObject;
        Debug.Log(opis);
        h = buffing.GetComponent<PlayerMovement>().getWeapon();
        h.recoil /= 3f;
        buffing.GetComponent<PlayerMovement>().makeWeapon(h);
    }

    override protected void Deffect()
    {
        z = buffing.GetComponent<PlayerMovement>().getWeapon();
        if(h==z)
        {
            h.recoil *= 3f;
            buffing.GetComponent<PlayerMovement>().makeWeapon(h);
        }
        
        Debug.Log(opis + "off");
    }
}
