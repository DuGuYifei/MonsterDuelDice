using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_11 : MonoBehaviour
{
    private GameObject[] smallMonster;
    private GameObject largeMonster;

    private bool flag_FindingMaterial = false;  //正在查找素材
    //private bool flag_FindedMaterial = false;   //已经找到了素材

    private bool playerIndexOdd;

    private int cellX;                          //骰子投放的格子
    private int cellY;
    private int x, y;                           //鼠标点击的素材的坐标

    private int monsterLevelIndex;              //被融合的怪物的索引，便于献祭召唤出下一等级的怪

    private string monsterMergedName;           //被融合的怪物的名字

    //private LayerMask checkerboardLayer;        //获得射线碰撞的棋盘所在的层
    private RaycastHit checkerboard;            //射线射到的棋盘

    private CellFunction cellFunction = new CellFunction();

    private void Awake()
    {
        //checkerboardLayer = LayerMask.GetMask("Checkerboard");
        if (PlayerParameter.ActivePlayerIndex / 2 == (PlayerParameter.ActivePlayerIndex + 1) / 2)
        {
            playerIndexOdd = false;
        }
        //如果是P2
        else
        {
            playerIndexOdd = true;
        }
    }

    // Update is called once per framez
    void Update()
    {
        if (transform.position.y <= -ConstantParameter.diceHeight / 2f && !flag_FindingMaterial)
        {
            //得到融合骰子扔到了哪个cell
            cellX = (int)(transform.position.x + 7 * ConstantParameter.diceHeight);
            //如果是P2
            if (playerIndexOdd)
            {
                cellY = (int)transform.position.z - 495;
            }
            //如果是P1
            else
            {
                cellY = (int)transform.position.z;
            }

            monsterMergedName = CellParameter.CellInformation[cellX, cellY].Name;

            //2种情况:里面有物体，且是1-4级的怪，并且数量大于1
            //且得是自己的怪
            //如果存在这个1-4级的怪就满足第一个条件
            bool Flag_Lv1To4 = false;

            for (monsterLevelIndex = 0; monsterLevelIndex < 4; monsterLevelIndex++)
            {
                if (monsterMergedName == PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[monsterLevelIndex])
                {
                    Flag_Lv1To4 = true;
                    break;
                }
            }

            //同时满足两个条件就是对的
            if (Flag_Lv1To4 && PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[monsterMergedName].Num > 1 && CellParameter.CellInformation[cellX, cellY].PlayerIndex == PlayerParameter.ActivePlayerIndex)
            {
                flag_FindingMaterial = true;
                StaticGameObject.UIDiceParentObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
                //提示放弃融合骰子
            }
        }

        //如果满足了上述条件才会继续选择素材
        if(flag_FindingMaterial && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.allCameras[1].ScreenPointToRay(Input.mousePosition), out checkerboard, 100, 1<<3))
            {
                x = (int)(checkerboard.point.x + 7.5);

                //如果是P2
                if (playerIndexOdd)
                {
                    y = (int)checkerboard.point.z - 495;
                }
                //如果是P1
                else
                {
                    y = (int)checkerboard.point.z;
                }

                //Debug.Log("x:" + x + "y:" + y + "这个格子的信息是：" + CellParameter.CellInformation[x, y].ObjectInCell_S.name);
                //Debug.Log(CellParameter.CellInformation[x, y].Name + " " + monsterMergedName);
                //Debug.Log(x + " " + cellX + " " + y + " " + cellY);

                if (CellParameter.CellInformation[x, y].Name == monsterMergedName && (x != cellX || y != cellY) && CellParameter.CellInformation[x, y].PlayerIndex == PlayerParameter.ActivePlayerIndex)
                {
                    //破坏四个旧的小大小大的怪物
                    cellFunction.DestroyCellObject(cellX, cellY);
                    cellFunction.DestroyCellObject(x, y);

                    //召唤新的怪物
                    cellFunction.NewCellObject(PlayerParameter.ActivePlayerIndex, cellX, cellY, PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[monsterLevelIndex + 1]);
                    
                    ////更改cellparameter
                    ////更改被当作素材的格子的信息   更改升级的格子的信息0
                    //CellParameter.CellInformation[x, y].Name = ConstantParameter.EMPTYCELL;
                    //CellParameter.CellInformation[x, y].PlayerIndex = -1;
                    //CellParameter.CellInformation[x, y].ObjectProperty = null;
                    //CellParameter.CellInformation[x, y].ObjectInCell_S = null;
                    //CellParameter.CellInformation[x, y].ObjectInCell_L = null;

                    //Type t = Type.GetType(ObjectClassNameParameter.className[CellParameter.CellInformation[cellX, cellY].Name]);
                    //CellParameter.CellInformation[cellX, cellY].ObjectProperty = Activator.CreateInstance(t);
                    //CellParameter.CellInformation[cellX, cellY].Name = PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[monsterLevelIndex + 1];
                    ////（接下）还有一个cell参数在实例化大小怪物之后
                    ////Debug.Log(CellParameter.CellInformation[cellX, cellY].ObjectProperty.Hp);

                    ////召唤新的怪物
                    //smallMonster[0] = PlayerParameter.Player[0].MonsterDict[PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[monsterLevelIndex + 1]].PrefabOfMonster_S;
                    //smallMonster[1] = PlayerParameter.Player[0].MonsterDict[PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[monsterLevelIndex + 1]].PrefabOfMonster_S;
                    //largeMonster = PlayerParameter.Player[0].MonsterDict[PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[monsterLevelIndex + 1]].PrefabOfMonster_L;
                    //smallMonster[0].transform.position = new Vector3(transform.position.x, 0, cellY + 0.5f);
                    //smallMonster[1].transform.position = new Vector3(transform.position.x, 0, cellY + 0.5f + ConstantParameter.distance_P1_P2);
                    //largeMonster.transform.position = new Vector3(transform.position.x * ConstantParameter.times_GroundToBoard, ConstantParameter.battleGroundY, transform.position.z * ConstantParameter.times_GroundToBoard + ConstantParameter.battleGroundZ);

                    ////实例化怪物，小的在棋盘上，大的在战场上
                    //smallMonster[0] = Instantiate(smallMonster[0]);
                    //smallMonster[1] = Instantiate(smallMonster[1]);
                    //largeMonster = Instantiate(largeMonster);

                    ////（接上）cell最后几个参数
                    //CellParameter.CellInformation[cellX, cellY].ObjectInCell_S = smallMonster;
                    //CellParameter.CellInformation[cellX, cellY].ObjectInCell_L = largeMonster;

                    ////将怪物放到父物体上
                    //smallMonster[0].transform.parent = StaticGameObject.checkerboard_P1.transform;
                    //smallMonster[1].transform.parent = StaticGameObject.checkerboard_P2.transform;
                    //largeMonster.transform.parent = StaticGameObject.battleGround.transform;

                    ////flag_FindedMaterial = true;

                    StaticGameObject.UIDiceParentObject.SetActive(true);

                    Destroy(gameObject);
                }
            }
        }
    }
}
