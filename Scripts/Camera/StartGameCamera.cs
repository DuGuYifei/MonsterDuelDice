using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeCameraFieldOfView(65f, 1.5f));
    }

    IEnumerator ChangeCameraFieldOfView(float targetView, float time)
    {
        float view = Camera.main.fieldOfView;

        float velocity = (view - targetView) / time;

        //ÉãÏñ»ú·¶Î§µ÷Õû
        while (Camera.main.fieldOfView <= targetView)
        {
            Camera.main.fieldOfView -= velocity * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }
}
