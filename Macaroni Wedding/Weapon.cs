using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float bulletSpeed, dmg;
    public float shoot_timer, reload_weapon_timer,knockBack,recoil,shootingRange;
    public int number_bullets;
    public Weapon(float speed, float knock, float reload_timer, int bullets_number, float damage)
    {
        bulletSpeed = speed;
        knockBack = knock;
        reload_weapon_timer = reload_timer;
        number_bullets = bullets_number;
        shoot_timer = 0;
        dmg = damage;
    }

    public Weapon()
    {
        bulletSpeed = 30f;
        knockBack = 0.1f;
        reload_weapon_timer = 0.1f;
        number_bullets = 2;
        shootingRange = 15f;
        shoot_timer = 0;
        dmg = 10;
        recoil = 0.08f;

    }

    public Weapon(Weapon w)
    {
        bulletSpeed = w.bulletSpeed;
        knockBack = w.knockBack;
        reload_weapon_timer = w.reload_weapon_timer;
        number_bullets = w.number_bullets;
        shoot_timer = w.shoot_timer;
        dmg = w.dmg;
    }
}
