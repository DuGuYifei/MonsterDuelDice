using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Slider progressBar;
    public TMP_Text progressText;
    public GameObject buttons;

    private PlayableDirector settingButtonDirector;

    private GameObject cabinet;

    private Vector3 currentPositionVelocity;
    private float currentAngleVelocity;

    private const float smoothTime = 0.5f;

    void Awake()
    {
        settingButtonDirector = FindObjectOfType<PlayableDirector>();

        cabinet = GameObject.Find("Cabinet");
    }

    void Update()
    {
        if (StartMenuParameter.Flag_escapeToStartMenu)
        {
            StartMenuParameter.Flag_escapeToStartMenu = false;

            StopAllCoroutines();

            if (settingButtonDirector.time == 0f)
            {
                settingButtonDirector.time = settingButtonDirector.duration;
            }

            StartCoroutine(Rewind());

            //摄像机退回
            StartCoroutine(MoveCabinetCamera(8.68f, 3.7f, 4.88f, 0f));
        }
    }

    public void StartGameLoadScene()
    {
        StopAllCoroutines();

        buttons.SetActive(false);

        StartCoroutine(StartGame());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void settingButtonTimeLine()
    {
        StopAllCoroutines();

        settingButtonDirector.Pause();

        settingButtonDirector.Evaluate();

        settingButtonDirector.Resume();

        cabinet.GetComponent<CabinetMenu>().enabled = true;
    }

    IEnumerator StartGame()
    {
        AsyncOperation startGame = SceneManager.LoadSceneAsync("BattleGround");

        startGame.allowSceneActivation = false;

        StartCoroutine(MoveCabinetCamera(9.856f, 2.713f, 13.211f, 90f));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ChangeCameraFieldOfView(36f, 1f));

        //控制进度条 和 显示的字

        while (!startGame.isDone)
        {
            progressBar.value = startGame.progress;

            progressText.text = "Synchronization Rate: " + startGame.progress * 100 + "%";

            if (startGame.progress >= 0.9f)
            {
                progressBar.value = 1f;

                progressText.text = "Press AnyKey To Start...";
            }

            if (Input.anyKeyDown)
            {
                yield return StartCoroutine(ChangeCameraFieldOfView(1f, 1f));

                startGame.allowSceneActivation = true;
            }

            yield return null;
        }

    }


    IEnumerator ChangeCameraFieldOfView(float targetView, float time)
    {
        float view = Camera.main.fieldOfView;

        float velocity = (view - targetView) / time;

        //摄像机范围调整
        while (Camera.main.fieldOfView >= targetView)
        {
            Camera.main.fieldOfView -= velocity * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MoveCabinetCamera(float cameraX, float cameraY, float cameraZ, float cameraAngleY)
    {
        currentAngleVelocity = 0f;
        currentPositionVelocity = new Vector3(0f, 0f, 0f);

        yield return null;

        while (Camera.main.transform.position != new Vector3(cameraX, cameraY, cameraZ) || Camera.main.transform.eulerAngles.y != cameraAngleY)
        {
            Camera.main.transform.position = new Vector3
                (Mathf.SmoothDamp(Camera.main.transform.position.x, cameraX, ref currentPositionVelocity.x, smoothTime),
                Mathf.SmoothDamp(Camera.main.transform.position.y, cameraY, ref currentPositionVelocity.y, smoothTime),
                Mathf.SmoothDamp(Camera.main.transform.position.z, cameraZ, ref currentPositionVelocity.z, smoothTime));

            Camera.main.transform.eulerAngles = new Vector3(0f, Mathf.SmoothDampAngle(Camera.main.transform.eulerAngles.y, cameraAngleY, ref currentAngleVelocity, smoothTime));

            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator Rewind()
    {
        // 暂停并倒带
        settingButtonDirector.Pause();

        settingButtonDirector.time -= Time.deltaTime;
        settingButtonDirector.Evaluate();

        if (settingButtonDirector.time < 0f)
        {
            settingButtonDirector.time = 0f;
            settingButtonDirector.Evaluate();
        }
        else
        {
            yield return null;
            StartCoroutine(Rewind());
        }
    }

}
