using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float walkingSpeed, multiplier_enemy;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    AudioClip clip;
    [SerializeField]
    GameObject bar;
    float bar_reload;
    Animator ani;
    [SerializeField]
    GameObject missile;
    private Vector3 move, pull;
    private float h, v;
    private Vector3 shoot_direction;
    string joy;
    float bar_timer;
    private Weapon w;
    private Random rand;
    AudioSource audio;
    public GameObject gameManager;
    ScreenshakeManager screenshake;
    float cool;
    float cool_timer;


    public void makeWeapon(float speed, float recoil, float reload_timer, int bullets_number, float damage)
    {
        w = new Weapon(speed, recoil, reload_timer, bullets_number, damage);
    }

    public void makeWeapon(Weapon we)
    {
        w = we;
    }

    public Weapon getWeapon()
    {
        return w;
    }



    // Use this for initialization
    void Start()
    {
        screenshake = GameObject.Find("Main Camera").GetComponent<ScreenshakeManager>();
        cool = 0.15f;
        gameManager = GameObject.Find("GameManager");
        audio = GameObject.Find("audio").GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        bar_reload = 0.8f;
        rand = new Random();
        move = new Vector3();
        walkingSpeed = 0.2f;
        multiplier_enemy = 1;
        w = new Weapon(30f, 0.1f, 0.1f, 3, 10);
        w = new Weapon();
        if (gameObject.tag == "male")
            joy = "Joy1_";
        if (gameObject.tag == "female")
            joy = "Joy2_";
    }

    void FixedUpdate()
    {
        moveMe();
        shoot();
        //if(gameObject.tag=="female")
        
        animate();
        reset();
    }
    

    void animate()
    {
       
            ani.SetBool("Dragged", pull != Vector3.zero);
            ani.SetBool("Walking", move.magnitude == 1);
    }

    public void AddEnemy(Vector3 v, float f)
    {
        pull = v.magnitude > pull.magnitude ? v : pull;
        multiplier_enemy *= f;
    }

    private void reset()
    {
        pull = Vector3.zero;
        multiplier_enemy = 1;

        w.shoot_timer += Time.deltaTime;
        bar_timer+= Time.deltaTime;
        cool_timer += Time.deltaTime;
    }

    private void moveMe()
    {
        h = Input.GetAxis(joy + "1");
        v = Input.GetAxis(joy + "2") * -1;
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");
        move = new Vector3(h, 0, v);
        move.Normalize();


        GetComponent<Rigidbody>().MovePosition(transform.position + move * walkingSpeed * multiplier_enemy + pull);
    }

    private void shoot()
    {
        float a = Input.GetAxis(joy + 3);
        float b = Input.GetAxis(joy + 4) * -1;
        shoot_direction = new Vector3(a, 0, b);
        shoot_direction.Normalize();

        if ((gameObject.tag == "male" && a < 0) || (gameObject.tag == "female" && a > 0))
        {
            if (bar_timer > bar_reload)
            {

                AtakZBara(shoot_direction);
                bar_timer = 0;
            }
        }
        else
        if (Mathf.Abs(a) + Mathf.Abs(b) > 0.8f)
        {
            if (w.shoot_timer > w.reload_weapon_timer)
            {

                moveArm(shoot_direction);

                if(cool_timer>cool)
                {
                    audio.PlayOneShot(clip);
                    cool_timer = 0;
                }


                float range = w.recoil;

                bullet.GetComponent<BulletBehaviour>().changeDmg(w.dmg);
                Vector3 v3 = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
                Vector3 direction = v3;  //losowy wektor
                GameObject tmp = null;

                float oneAngle = w.shootingRange * 2 / (w.number_bullets-1);
                float actualAngle = -1 * w.shootingRange;

                if(w.number_bullets==1)
                {
                    direction = shoot_direction + v3;
                    tmp = (GameObject)Instantiate(bullet, missile.transform.position, Quaternion.identity);
                    tmp.GetComponent<Rigidbody>().velocity = direction * w.bulletSpeed;
                }
                else
                for(int i=0;i<w.number_bullets;i++)
                {
                    Vector3 where = missile.transform.position;
                    Quaternion matrix = Quaternion.AngleAxis(actualAngle, Vector3.up);

                    direction = shoot_direction + v3;
                    direction = matrix * direction;

                    v3 = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));

                    tmp = (GameObject)Instantiate(bullet, missile.transform.position + direction.normalized * 0.01f, Quaternion.identity);
                    
                    
                    tmp.GetComponent<Rigidbody>().velocity = direction * w.bulletSpeed;
                    actualAngle += oneAngle;

                }
                //Time.timeScale = 0;
                /*
                if (w.number_bullets > 1)
                {
                    for (int i = 0; i < w.number_bullets; i++)
                    {
                        GameObject tmp = (GameObject)Instantiate(bullet, missile.transform.position, Quaternion.identity);
                        Vector3 v4 = new Vector3((i / w.number_bullets) * 0.1f - 0.05f, 0, ((i / w.number_bullets) * 0.1f - 0.05f) * w.bulletSpeed);
                        tmp.GetComponent<Rigidbody>().velocity = (shoot_direction + v3 +v4)* w.bulletSpeed;
                    }
                }
                else
                {*/




                ScreenshakeManager Shaker;
                if (Shaker = Camera.main.GetComponent<ScreenshakeManager>())
                    Shaker.Shake(0.08f, 0.07f);
                KnockBack(shoot_direction);

                w.shoot_timer = 0;
            }
        }
        //else
            //audio.Stop();
    }

    private void KnockBack(Vector3 v)
    {
        transform.position -= v * w.knockBack;
    }

    void moveArm(Vector3 dir)
    {
        Vector3 vec = Vector3.left;
        if (gameObject.tag == "male")
            vec=Vector3.right;
        Transform arm = transform.Find("Sprite/ArmPivot");
        float angle = Vector3.Angle(dir, vec);
        if (dir.z < 0 && gameObject.tag == "male") angle *= -1;
        if (dir.z > 0 && gameObject.tag == "female") angle *= -1;
        arm.eulerAngles = new Vector3(0, 0, angle);
        missile.transform.position = arm.transform.position + dir * 0.2f ;

    }

    void AtakZBara(Vector3 direction)
    {
       // Debug.Log("ATAK Z BARA");

        GameObject tmp = (GameObject)Instantiate(bar, transform.position, Quaternion.identity);
        tmp.transform.position = transform.position + direction * 0.3f;
        transform.position -= direction * 0.1f;

        ScreenshakeManager Shaker;
        if (Shaker = Camera.main.GetComponent<ScreenshakeManager>())
            Shaker.Shake(0.1f,0.1f);
        KnockBack(shoot_direction);

        Destroy(tmp, 0.2f);
    }

}

