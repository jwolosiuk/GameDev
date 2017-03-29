using UnityEngine;

public class Crystal : MonoBehaviour
{
    public float MIN_SCALE_X = 1.1f;
    public float MAX_SCALE_X = 2.4f;
    public float MIN_DISTANCE_LEFT = 0;
    public float MIN_DISTANCE_RIGHT = 0;

    virtual public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.eventSystem.Call("PlayerDied", GameManager.eventSystem);
        }
    }

    virtual public GameObject Spawn(float posX, float lowerY, float upperY, float posZ)
    {
        GameObject newObject;
        newObject = Instantiate(gameObject, new Vector3(posX, lowerY, posZ), Quaternion.identity, transform.parent);
        newObject.transform.localScale = GenerateScale();
        newObject.transform.eulerAngles = GenerateEulerRotation(new Vector3());
        return newObject;
    }

    virtual public Vector3 GenerateScale()
    {
        float scale = Random.Range(MIN_SCALE_X, MAX_SCALE_X);
        return new Vector3(scale, scale, scale);
    }

    virtual public Vector3 GenerateEulerRotation(Vector3 euler)
    {

        euler.y = Random.Range(0.0f, 360.0f);
        euler.x = Random.Range(-12.0f, 12.0f);
        euler.z = Random.Range(-12.0f, 12.0f);
        return euler;
    }
}
