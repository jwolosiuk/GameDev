using UnityEngine;
using System.Collections;

public class ScreenshakeManager : MonoBehaviour
{
    bool shaking = false;

    [SerializeField]
    float ShakingRadius = 0.03f;

    [SerializeField]
    float ShakingDensity = 0.01f;
    bool fading = true;
    float Timer = 0f;
    const float returner = 0.4f;

    public bool isExplosion = false;

    Vector3 InitialPose;

	// Use this for initialization
	void Start ()
    {
        InitialPose = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(shaking)
            Timer += Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, InitialPose, returner);

        if(Timer>=ShakingDensity && shaking)
        {
            Timer = 0f;
            CamShake();
        }
	}

    void CamShake()
    {
        Vector3 rand = Random.insideUnitSphere;
        rand *= ShakingRadius;
        if(fading) ShakingRadius *= 0.97f;
        transform.position += rand;
    }

    public void StopShaking()
    {
        shaking = false;
        isExplosion = false;
        ShakingRadius = 0.03f;
    }

    public void StartShaking()
    {
        shaking = true;
        
    }

    public bool isShaking()
    {
        return shaking;
    }

    public void Shake(float timelenght,float shakingRadius=0.03f, bool isGranade=false)
    {
        isExplosion = isGranade;
        if (shaking == false)
        {
            StartShaking();
            ShakingRadius =shakingRadius;
            Invoke("StopShaking", timelenght);
        }
        else
        if(isGranade)
        {
            CancelInvoke("StopShaking");
            ShakingRadius = shakingRadius;
            fading = true;
            Invoke("StopShaking", timelenght);
        }
        
    }
}
