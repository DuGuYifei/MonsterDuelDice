using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceOnLocating : MonoBehaviour
{
    private string uiButtonName;            //这个骰子是由哪个按钮召唤出来的

    private bool locating = true;           //正在寻地点

    //private LayerMask checkerboardLayer;
    private RaycastHit checkerboard;

    public string UIButtonName { get => uiButtonName; set => uiButtonName = value; }

    //private void Awake()
    //{
    //    checkerboardLayer = LayerMask.GetMask("Checkerboard"); 
    //}

    void Update()
    {
        if (locating)
        {
            if (Physics.Raycast(Camera.allCameras[1].ScreenPointToRay(Input.mousePosition), out checkerboard, 100, 1<<3))
            {
                transform.position = checkerboard.point + ConstantParameter.diceHeight * new Vector3(0f, 1f, 0f);
            }
            if (Input.GetMouseButtonUp(0))
            {
                locating = false;
                //如果超过P1和P2的棋盘就destroy掉
                if (transform.position.x <= -7.5f || transform.position.x > 7.5f || transform.position.z < 0f || (transform.position.z > 15f && transform.position.z < 495f) || transform.position.z > 510f
                    || Input.GetMouseButtonDown(1)          //右键取消
                    )
                {
                    Destroy(gameObject);
                }
                //不超出则
                else
                {
                    SummonDiceParameter.Located[UIButtonName] = true;
                    float gridX = (int)(checkerboard.point.x + 7.5f) - 7;
                    float gridZ = (int)checkerboard.point.z + 0.5f; 
                    transform.position = new Vector3(gridX, checkerboard.point.y + ConstantParameter.diceHeight * 0.5f, gridZ); 
                    StartCoroutine("DiceElevateDown");
                }
            }
        }
    }

    //让骰子沉到棋盘下面
    IEnumerator DiceElevateDown()
    {
        while (transform.position.y > -ConstantParameter.diceHeight / 2f)
        {
            transform.position -= new Vector3(0f, 0.02f, 0f);
            yield return new WaitForFixedUpdate();
        }
    }
}
