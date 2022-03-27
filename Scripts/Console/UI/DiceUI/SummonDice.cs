using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SummonDice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CanvasGroup panelUIdice;             //放上UIdice的父物体panel方便调整所有uidice透明度
    public GameObject[] mergeButton;            //如果用完一个骰子让融合按钮消失
    //private bool callDice = false;              //如果该uiDice正在召唤骰子则改为true,静态变量located是用来判定是否放弃先使用该骰子

    private const float diceHeight = 1f;

    private string[] summonDicePath = { "Prefab/Dice/dragDice/Dice01_1",
                                        "Prefab/Dice/dragDice/Dice01_2",
                                        "Prefab/Dice/dragDice/Dice01_3",
                                        "Prefab/Dice/dragDice/Dice01_4",
                                        "Prefab/Dice/dragDice/Dice01_5",
                                        "Prefab/Dice/dragDice/Dice01_6" };


    //private LayerMask checkerboardLayer;        //获得射线碰撞的棋盘所在的层
    private RaycastHit checkerboard;            //射线射到的棋盘
    private GameObject dragDicePrefab;          //要召唤的骰子
    private int newDiceIndex = -1;              //召唤完骰子要将静态变量newdice转为false,12代表1和2的骰子所以从1开始不是0

    private void Awake()
    {
        //checkerboardLayer = LayerMask.GetMask("Checkerboard");
        SummonDiceParameter.Located.Add(gameObject.name, false);
    }

    void Update()
    {
        //如果找到位置了即使用过了该骰子，则把按钮关掉和相关融合按钮关掉
        if (SummonDiceParameter.Located[gameObject.name])
        {
            foreach (GameObject button in mergeButton)
            {
                button.SetActive(false);
            }

            if (newDiceIndex <= 3)
            {
                ThrowDiceParameter.NewDice[newDiceIndex - 1] = false;
            }
            else
            {
                switch (newDiceIndex)
                {
                    case 12:
                        ThrowDiceParameter.NewDice[0] = false;
                        ThrowDiceParameter.NewDice[1] = false;
                        break;
                    case 23:
                        ThrowDiceParameter.NewDice[1] = false;
                        ThrowDiceParameter.NewDice[2] = false;
                        break;
                    case 123:
                        ThrowDiceParameter.NewDice[0] = false;
                        ThrowDiceParameter.NewDice[1] = false;
                        ThrowDiceParameter.NewDice[2] = false;
                        break;
                }
            }

            newDiceIndex = -1;
            SummonDiceParameter.Located[gameObject.name] = false;
            gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //决定召唤哪个骰子
            SwitchSummonDice(this.name, out dragDicePrefab, ref newDiceIndex);
            //初始化召唤的位置
            if (Physics.Raycast(Camera.allCameras[1].ScreenPointToRay(Input.mousePosition), out checkerboard, 100, 1<<3))
            {
                //Debug.Log(Camera.allCameras[1].name);
                dragDicePrefab.transform.position = checkerboard.point + diceHeight * new Vector3(0f, 1f, 0f);
            }

            //召唤骰子
            Quaternion afterRotate = dragDicePrefab.transform.rotation; //是否需要旋转180度

            //P2要旋转180度
            if (PlayerParameter.ActivePlayerIndex / 2 != (PlayerParameter.ActivePlayerIndex + 1) / 2)
            {
                afterRotate = dragDicePrefab.transform.rotation;
                afterRotate.eulerAngles += new Vector3(0f, 180f, 0f);
            }

            dragDicePrefab.GetComponent<DiceOnLocating>().UIButtonName = gameObject.name;

            GameObject dragDice = Instantiate(dragDicePrefab, dragDicePrefab.transform.position, afterRotate);

            dragDice.GetComponent<DiceOnLocating>().UIButtonName = gameObject.name;

            //关掉prefab上的所有脚本组件
            MonoBehaviour[] scriptArray = dragDicePrefab.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour mb in scriptArray)
            {
                mb.enabled = false;
            }

            //callDice = true;
            panelUIdice.alpha = 0.2f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left
            //&& !SummonDiceParameter.Located
            )
        {
            //callDice = false;
            panelUIdice.alpha = 1f;
        }
    }

    private void SwitchSummonDice(string uiName, out GameObject dragDice, ref int newDiceIndex)
    {
        int throwResult = 0;    //根据对应的结果选择骰子和应有的脚本
        int mergeNum = 1;       //根据融合了几个骰子挑选脚本
        switch (uiName)
        {
            //融合的均取第二个值(dice throw result)
            case "UIDice_1":
                newDiceIndex = 1;
                throwResult = ThrowDiceParameter.DiceThrowResult[0];
                break;
            case "UIDice_2":
                newDiceIndex = 2;
                throwResult = ThrowDiceParameter.DiceThrowResult[1];
                break;
            case "UIDice_3":
                newDiceIndex = 3;
                throwResult = ThrowDiceParameter.DiceThrowResult[2];
                break;
            case "UIDice_1_2":
                newDiceIndex = 12;
                throwResult = ThrowDiceParameter.DiceThrowResult[1];
                mergeNum = 2;
                break;
            case "UIDice_2_3":
                newDiceIndex = 23;
                throwResult = ThrowDiceParameter.DiceThrowResult[1];
                mergeNum = 2;
                break;
            case "UIDice_1_2_3":
                newDiceIndex = 123;
                throwResult = ThrowDiceParameter.DiceThrowResult[1];
                mergeNum = 3;
                break;
        }

        dragDice = Resources.Load<GameObject>(summonDicePath[throwResult - 1]);     //初始化按照的点数需要的骰子
        //dragDice.AddComponent<DiceOnLocating>();                                  //使召唤出的骰子有寻路脚本
        dragDice.GetComponent<DiceOnLocating>().enabled = true;

        //根据融合数量和点数挑选合适的效果脚本
        switch (mergeNum)
        {
            case 1:
                switch (throwResult)
                {
                    case 1:
                        //dragDice.AddComponent<Point_1>();
                        dragDice.GetComponent<Point_1>().enabled = true;
                        return;
                    case 2:
                        //dragDice.AddComponent<Point_2>();
                        dragDice.GetComponent<Point_2>().enabled = true;
                        return;
                    case 3:
                        //dragDice.AddComponent<Point_3>();
                        dragDice.GetComponent<Point_3>().enabled = true;
                        return;
                    case 4:
                        //dragDice.AddComponent<Point_4>();
                        dragDice.GetComponent<Point_4>().enabled = true;
                        return;
                    case 5:
                        //dragDice.AddComponent<Point_5>();
                        dragDice.GetComponent<Point_5>().enabled = true;
                        return;
                    case 6:
                        //dragDice.AddComponent<Point_6>();
                        dragDice.GetComponent<Point_6>().enabled = true;
                        return;
                }
                return;
            case 2:
                switch (throwResult)
                {
                    case 1:
                        //dragDice.AddComponent<Point_11>();
                        dragDice.GetComponent<Point_11>().enabled = true;
                        return;
                    case 2:
                        //dragDice.AddComponent<Point_22>();
                        dragDice.GetComponent<Point_22>().enabled = true;
                        return;
                    case 3:
                        //dragDice.AddComponent<Point_33>();
                        dragDice.GetComponent<Point_33>().enabled = true;
                        return;
                    case 4:
                        //dragDice.AddComponent<Point_44>();
                        dragDice.GetComponent<Point_44>().enabled = true;
                        return;
                    case 5:
                        //dragDice.AddComponent<Point_55>();
                        dragDice.GetComponent<Point_55>().enabled = true;
                        return;
                    case 6:
                        //dragDice.AddComponent<Point_66>();
                        dragDice.GetComponent<Point_66>().enabled = true;
                        return;
                }
                return;
            case 3:
                switch (throwResult)
                {
                    case 1:
                        //dragDice.AddComponent<Point_111>();
                        dragDice.GetComponent<Point_111>().enabled = true;
                        return;
                    case 2:
                        //dragDice.AddComponent<Point_222>();
                        dragDice.GetComponent<Point_222>().enabled = true;
                        return;
                    case 3:
                        //dragDice.AddComponent<Point_333>();
                        dragDice.GetComponent<Point_333>().enabled = true;
                        return;
                    case 4:
                        //dragDice.AddComponent<Point_444>();
                        dragDice.GetComponent<Point_444>().enabled = true;
                        return;
                    case 5:
                        //dragDice.AddComponent<Point_555>();
                        dragDice.GetComponent<Point_555>().enabled = true;
                        return;
                    case 6:
                        //dragDice.AddComponent<Point_666>();
                        dragDice.GetComponent<Point_666>().enabled = true;
                        return;
                }
                return;
        }
    }
}
