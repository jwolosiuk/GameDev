using UnityEngine;
using System.Collections;

public class GranadeBehav : MonoBehaviour {

    public AudioClip sound;

    void OnTriggerEnter(Collider col)
    {
        //animacje
        EnemyHealth CollidedHealth;
        if (col.gameObject.tag == "enemy_female" || col.gameObject.tag == "enemy_male")
            if (CollidedHealth = col.GetComponent<EnemyHealth>())
            {
                CollidedHealth.TakeDamage(2f,3f);

            }


    }
}
