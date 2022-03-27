using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CabinetMenu : MonoBehaviour
{
    private PlayableDirector settingButtonDirector;

    private RaycastHit cabinet;
    private int lastClicked;

    private bool flag_canAdjistPlatformNotOpenBlock;        //打开区域快不再能调整平台
    private bool flag_startDragPlatform;                    //开始调整平台

    private Vector3 currentVelocity;

    private const float smoothTime = 0.5f;

    private Vector3 lastFramePointPosition, newLocalPosition;

    private GameObject[] MonsterText;

    private GameObject[] TrapText;

    private GameObject PlayerAbilityText;

    private bool firstFrame = true;

    // Start is called before the first frame update
    void Awake()
    {
        MonsterText = GameObject.FindGameObjectsWithTag("MonsterText");
        TrapText = GameObject.FindGameObjectsWithTag("TrapText");
        PlayerAbilityText = GameObject.FindGameObjectWithTag("PlayerAbilityText");
    }

    void Start()
    {
        flag_canAdjistPlatformNotOpenBlock = true;
        flag_startDragPlatform = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(firstFrame)
        {
            firstFrame = false;
            StartCoroutine(CameraSmoothMove(8.11f, 3.76f, 16.6f));
            StartCoroutine(SmoothMoveCameraAngle(-90f));
        }

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out cabinet, 100, 1 << 9))
        {
            //如果是平台就可以调整位置
            if (flag_canAdjistPlatformNotOpenBlock && cabinet.collider.name == "Platform")
            {
                flag_startDragPlatform = true;
                lastFramePointPosition = cabinet.point;
            }

            //如果选择了区域就打开去选择
            else if (cabinet.collider.name.EndsWith("Block"))
            {
                //如果两次点击不一样
                if (lastClicked != cabinet.collider.GetHashCode())
                {
                    flag_canAdjistPlatformNotOpenBlock = false; //打开区域后不能再调整平台

                    OpenBlock();

                    lastClicked = cabinet.collider.GetHashCode();
                }

                //如果两次点击一样
                //1.如果是怪兽层 
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out cabinet, 100, 1 << 10))
                {
                    int lastElement = int.Parse(cabinet.collider.name.Substring(cabinet.collider.name.Length - 1));

                    MonsterText[lastElement - 1].GetComponent<Text>().text = cabinet.collider.name;

                }

                //2.如果是陷阱层
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out cabinet, 100, 1 << 11))
                {
                    int lastElement = int.Parse(cabinet.collider.name.Substring(cabinet.collider.name.Length - 1));
                    //Debug.Log(cabinet.collider.name.Substring(0, cabinet.collider.name.Length - 1));
                    TrapText[lastElement].GetComponent<Text>().text = cabinet.collider.name.Substring(0, cabinet.collider.name.Length - 1);

                    
                }

                //3.如果是玩家技能层
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out cabinet, 100, 1 << 12))
                {
                    PlayerAbilityText.GetComponent<Text>().text = cabinet.collider.name;
                }
            }
        }

        else if (Input.GetMouseButtonUp(0) && flag_startDragPlatform)
        {
            flag_startDragPlatform = false;
        }

        //调整平台
        else if (flag_startDragPlatform && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out cabinet, 100, 1 << 9))
        {
            AdjustPlatform();
        }

        //退出block选区
        else if (!flag_canAdjistPlatformNotOpenBlock && Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            flag_canAdjistPlatformNotOpenBlock = true;
            StartCoroutine(CameraSmoothMove(8.11f, 3.76f, 16.6f));
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();

            StartMenuParameter.Flag_escapeToStartMenu = true;

            firstFrame = true;

            gameObject.GetComponent<CabinetMenu>().enabled = false;
        }
    }

    IEnumerator SmoothMoveCameraAngle(float cameraAngleY)
    {
        float currentAngleVelocity = 0f;

        yield return null;

        while (Camera.main.transform.eulerAngles.y != cameraAngleY)
        {
            Camera.main.transform.eulerAngles = new Vector3(0f, Mathf.SmoothDampAngle(Camera.main.transform.eulerAngles.y, cameraAngleY, ref currentAngleVelocity, smoothTime));

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CameraSmoothMove(float cameraX, float cameraY, float cameraZ)
    {
        currentVelocity = new Vector3(0f, 0f, 0f);

        yield return null;

        while (Camera.main.transform.position != new Vector3(cameraX, cameraY, cameraZ))
        {
            Camera.main.transform.position = new Vector3
                (Mathf.SmoothDamp(Camera.main.transform.position.x, cameraX, ref currentVelocity.x, smoothTime),
                Mathf.SmoothDamp(Camera.main.transform.position.y, cameraY, ref currentVelocity.y, smoothTime),
                Mathf.SmoothDamp(Camera.main.transform.position.z, cameraZ, ref currentVelocity.z, smoothTime));

            yield return new WaitForFixedUpdate();
        }
    }

    private void OpenBlock()
    {
        float cameraX = 4.68f;
        float cameraZ = 0, cameraY = 0;
        if (cabinet.collider.name.StartsWith("Left"))
        {
            cameraZ = 14.2f;
        }
        else if (cabinet.collider.name.StartsWith("Middle"))
        {
            cameraZ = 15.3f;
        }
        else if (cabinet.collider.name.StartsWith("Right"))
        {
            cameraZ = 16.5f;
        }

        if (cabinet.collider.name.EndsWith("TopBlock"))
        {
            cameraY = 4.58f;
        }
        else if (cabinet.collider.name.EndsWith("MiddleBlock"))
        {
            cameraY = 2.77f;
        }
        else if (cabinet.collider.name.EndsWith("BottomBlock"))
        {
            cameraY = 1.36f;
        }

        StopAllCoroutines();

        StartCoroutine(CameraSmoothMove(cameraX, cameraY, cameraZ));
    }

    private void AdjustPlatform()
    {
        //增加跟上一次鼠标位置的y的变化量，且控制在一定范围内防止有一层放不下东西
        newLocalPosition = cabinet.collider.transform.parent.localPosition + new Vector3(0f, (cabinet.point.y - lastFramePointPosition.y));
        if ((newLocalPosition.y <= 2f && newLocalPosition.y >= 1.5f) || (newLocalPosition.y <= 3.5f && newLocalPosition.y >= 3f))
        {
            cabinet.collider.transform.parent.localPosition = newLocalPosition;
        }
        lastFramePointPosition = cabinet.point;
    }
}
