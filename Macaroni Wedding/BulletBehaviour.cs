using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;

    [SerializeField]
    string damageTag = "";

    public void changeDmg(float d)
    {
        damage = d;
    }
    void OnTriggerEnter(Collider col)
    {
        //animacje
        EnemyHealth CollidedHealth;
        if (col.gameObject.tag ==  damageTag)
        if (CollidedHealth = col.GetComponent<EnemyHealth>())
        {
            CollidedHealth.TakeDamage(damage);
            DestroyBullet();

        }
        
        
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
        ScreenshakeManager Shaker;
        if(Shaker = Camera.main.GetComponent<ScreenshakeManager>())
            Shaker.Shake(0.2f);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
