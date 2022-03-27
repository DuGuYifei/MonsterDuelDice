using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraView : MonoBehaviour
{
    private float velocityX = 0f, velocityY = 0f;           //ÉãÏñ»úµÄxyRotation
    private Vector3 startAngle;

    Camera currentCamera;

    float startFov, minFov = 10f, maxFov = 72f;

    void Awake()
    {
        startAngle = transform.eulerAngles;

        currentCamera = gameObject.GetComponent<Camera>();
        startFov = currentCamera.fieldOfView;

        FindObjectOfType<RoundControl>().NextRound += CameraReset;
    }

    private void CameraReset()
    {
        transform.eulerAngles = startAngle;
        currentCamera.fieldOfView = startFov;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            CameraReset();
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            velocityX -= 1f;
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            velocityX += 1f;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            velocityY -= 1f;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            velocityY += 1f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            velocityX += 1f;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            velocityX -= 1f;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            velocityY += 1f;

        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            velocityY -= 1f;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float fov = currentCamera.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * 20f;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            currentCamera.fieldOfView = fov;
        }
    }

    void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(velocityX, velocityY, 0f);
    }
}
