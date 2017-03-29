using UnityEngine;
using System.Collections;

public class granade : AnyBuff {

    AudioSource audio;
    GameObject granat, tmp;
    void Start()
    {
        audio = GameObject.Find("audio").GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        time = 0.2f;
        opis = "Fucking Granade!";
        granat = (GameObject)Resources.Load("granat");
        AffectSound = granat.GetComponent<GranadeBehav>().sound;
    }
	override protected void Affect(Collider col)
    {
        buffing = col.gameObject;
        audio.PlayOneShot(AffectSound);
        tmp = (GameObject)Instantiate(granat, transform.position - Vector3.up*2, Quaternion.identity);

        ScreenshakeManager Shaker;
        if (Shaker = Camera.main.GetComponent<ScreenshakeManager>())
            Shaker.Shake(3f,3f,true);

        Debug.Log(opis);
    }

    override protected void Deffect()
    {

        Destroy(tmp);
        Debug.Log(opis + "off");
    }
}
