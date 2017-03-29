using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBall : MonoBehaviour {


    public float playerDifferenceSpeed;
    public float maxExpandigValue;
    public float expandingSpeed;
    Material soundDistortion;

    float bulletSpeed;

    private void Start()
    {
        gameObject.transform.localScale = new Vector3(0f, 0f, 1f);
        soundDistortion = GetComponent<MeshRenderer>().material;
        bulletSpeed = GameManager.instance.gameSpeed - playerDifferenceSpeed;
    }

    private void Update()
    {
        gameObject.transform.localScale += new Vector3(expandingSpeed * Time.deltaTime, expandingSpeed * Time.deltaTime, 0);
        gameObject.transform.position += new Vector3(bulletSpeed*Time.deltaTime,0f,0f);

        //Debug.Log(1f - transform.localScale.x / maxExpandigValue);
        soundDistortion.SetFloat("_Fade",1f - transform.localScale.x / maxExpandigValue);

        if (gameObject.transform.localScale.x > maxExpandigValue)
            Destroy(gameObject);
            
    }



    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.gameObject.tag == "Crystal")
        {
            GameManager.instance.PlaySound("Crystals2");
        }
        else if(other.gameObject.tag == "SoundCrystal")
        {
            Debug.Log("Destroy!");
            other.gameObject.GetComponentInParent<SoundCrystal>().DestroyMe();
            GameManager.instance.PlaySound("RockDead");
        }
    }


}
