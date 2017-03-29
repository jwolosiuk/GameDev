using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : Culmination {

    public float cameraTurnAngle;
    public float desiredSize;

    public float effectSpeedPosition;
    public float effectSpeedRotation;
    public float effectSpeedZoom;

    public float epsilonRotation;
    public float epsilonPosition;
    public float epsilonZoom;

    //Make szaders great again 
    //Need Blur for camera and monochrome (black and white)

    enum States { NotPlaying, Playing, Returning, Die, numOfStates };
    States state = States.NotPlaying;

    Vector3 cameraPosition;
    Vector3 playerPosition;
    Transform camTransform;
    Camera camCamera;

    float currentCameraTurn = 0;
    float cameraZoom;
    float currentZoom; 
    


    void Awake () {
        camTransform = GameManager.mainCamera.transform;
        state = States.NotPlaying;
        currentCameraTurn = 0;
        camCamera = GameManager.mainCamera.GetComponent<Camera>();
        cameraZoom = currentZoom = camCamera.orthographicSize;

        Play();  
	}
	
	
	void Update () {
		if(state == States.Playing)
        {
             //And animate Zoom but that later 
            //As well as animate black and white shader to be stronger with time
            if (RotateTo(cameraTurnAngle) & MoveTo(playerPosition) & ZoomTo(desiredSize))
                state = States.Returning;
        }
        if(state == States.Returning)
        {
            //And animate Zoom but that later 
            //As well as animate black and white shader to be less strong with time 
            if (RotateTo(0) & MoveTo(cameraPosition) & ZoomTo(cameraZoom))
                state = States.Die;
        }
        if (state == States.Die)
        {
            camTransform.rotation = Quaternion.Euler(0, 0, 0);
            camTransform.position = cameraPosition;
            camCamera.orthographicSize = cameraZoom;
            Destroy(gameObject);
        }
	}

    public override void Play()
    {
        state = States.Playing;
        cameraPosition = camTransform.position;
        //take player position from Game Manager or other shit I don't care 
        playerPosition = GameManager.player.transform.position;
        //enable shader
    }

    bool RotateTo(float angle)
    {
        currentCameraTurn = Mathf.LerpAngle(currentCameraTurn, angle, effectSpeedRotation * Time.deltaTime);
        camTransform.rotation = Quaternion.Euler(0, 0, currentCameraTurn);
        return  Mathf.Abs(currentCameraTurn - angle) < epsilonRotation;
    }

    bool MoveTo(Vector3 position)
    {
        Vector3 newPosition = Vector3.Lerp(camTransform.position, position, effectSpeedPosition * Time.deltaTime);
        newPosition.z = cameraPosition.z;
        camTransform.position = newPosition;
        Vector2 pos = new Vector2(position.x, position.y);
        Vector2 camPos = new Vector2(newPosition.x, newPosition.y);

        
        return Vector2.Distance(pos, camPos) < epsilonPosition;
    }

    bool ZoomTo(float zoom)
    {
        currentZoom = Mathf.Lerp(currentZoom, zoom, effectSpeedZoom * Time.deltaTime);
        camCamera.orthographicSize = currentZoom;
        return Mathf.Abs(camCamera.orthographicSize - zoom) < epsilonZoom ;
    }
}
