using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMechanic : MonoBehaviour
{
    private const float timeBefor = 2.5f;
    
    public Transform lookAt;

    private Vector3 desiredPosition;
    private Vector3 offset;

    private Vector2 touchPosition;
    private float swipeResistance = 200.0f;

    private float smoothSpeed = 7.5f;
    private float distance = 5.0f;
    private float yOffset = 3.5f;

    private float startTime = 0;

    private void Start()
    {
        offset = new Vector3(0, yOffset, -1f * distance);
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - startTime < timeBefor)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SlideCamera(true);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SlideCamera(false);

        if(Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1))
        {
            touchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            float swipeForce = touchPosition.x - Input.mousePosition.x;
            
            if(Mathf.Abs (swipeForce) > swipeResistance)
            {
                if (swipeForce < 0)
                    SlideCamera(true);
                else
                    SlideCamera(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - startTime < timeBefor)
        {
            return;
        }

        desiredPosition = lookAt.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); 
        transform.LookAt(lookAt.position + Vector3.up);
    }

    public void SlideCamera (bool left)
    {
        if (left)
            offset = Quaternion.Euler(0, 90, 0) * offset;
        else
            offset = Quaternion.Euler(0, -90, 0) * offset;
    }
}
