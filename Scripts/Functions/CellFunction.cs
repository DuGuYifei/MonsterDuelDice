using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//格子信息的更新 和 格子怪物的修改
public class CellFunction
{
    public void DestroyCellObject(int cellX, int cellY)
    {
        UnityEngine.Object.Destroy(CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[0]);
        UnityEngine.Object.Destroy(CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[1]);
        UnityEngine.Object.Destroy(CellParameter.CellInformation[cellX, cellY].ObjectInCell_L);

        ClearCellObjectInfo(cellX, cellY);
    }

    public void NewCellObject(int playerIndex, int cellX, int cellY, string monster_S_Name)
    {
        GameObject[] smallMonster = new GameObject[2];
        GameObject largeMonster = null;
        GameObject smallMonsterPrefab = PlayerParameter.Player[playerIndex].MonsterDict[monster_S_Name].PrefabOfMonster_S;
        GameObject largeMonsterPrefab = PlayerParameter.Player[playerIndex].MonsterDict[monster_S_Name].PrefabOfMonster_L;

        ObjectInstantiate(playerIndex, cellX, cellY, smallMonsterPrefab, largeMonsterPrefab, ref largeMonster,ref smallMonster);

        //Vector3[] smallMonsterPosition = new Vector3[2];
        //Vector3 largeMonsterPosition;
        //Quaternion afterRotate = smallMonsterPrefab.transform.rotation; //是否需要旋转180度

        ////召唤怪兽
        ////P1
        //smallMonsterPosition[0] = new Vector3(cellX - 7f, 0, cellY + 0.5f);
        //smallMonsterPosition[1] = new Vector3(cellX - 7f, 0, cellY + 0.5f + ConstantParameter.distance_P1_P2);
        //largeMonsterPosition = new Vector3(smallMonsterPosition[0].x * ConstantParameter.times_GroundToBoard, ConstantParameter.battleGroundY, smallMonsterPosition[0].z * ConstantParameter.times_GroundToBoard + ConstantParameter.battleGroundZ);

        ////P2 是奇数索引
        ////改变转向
        //if (playerIndex / 2 != (playerIndex + 1) / 2)
        //{
        //    afterRotate = smallMonsterPrefab.transform.rotation;
        //    afterRotate.eulerAngles += new Vector3(0f, 180f, 0f);
        //}

        ////实例化怪物，小的在棋盘上，大的在战场上
        //smallMonster[0] = UnityEngine.Object.Instantiate(smallMonsterPrefab, smallMonsterPosition[0], afterRotate);
        //smallMonster[1] = UnityEngine.Object.Instantiate(smallMonsterPrefab, smallMonsterPosition[1], afterRotate);
        //largeMonster = UnityEngine.Object.Instantiate(largeMonster, largeMonsterPosition, afterRotate);

        ////将怪物放到父物体上
        //smallMonster[0].transform.parent = StaticGameObject.checkerboard_P1.transform;
        //smallMonster[1].transform.parent = StaticGameObject.checkerboard_P2.transform;
        //largeMonster.transform.parent = StaticGameObject.battleGround.transform;

        Type t = Type.GetType(ObjectClassNameParameter.className[monster_S_Name]);
        dynamic objectProperty = Activator.CreateInstance(t);

        Slider[] hpSliders = new Slider[2] { smallMonster[0].GetComponentInChildren<Slider>(), smallMonster[1].GetComponentInChildren<Slider>() };

        //使血条对准摄像机
        for(int i = 0;i< 2;i++)
        {
            hpSliders[i].transform.rotation = Quaternion.LookRotation(ConstantParameter.PlayerCameraPosition[i]  - new Vector3(0f, hpSliders[i].transform.position.y, hpSliders[i].transform.position.z)); //= new Vector3(0f, 180f, 0f);

            //再翻转180度
            hpSliders[i].transform.eulerAngles += new Vector3(0f, 0f, 180f);
        }


        objectProperty.HpSlider = hpSliders;

        AddCellObjectInfo(playerIndex, cellX, cellY, monster_S_Name, smallMonster, largeMonster, objectProperty);
    }

    public void ObjectInstantiate(int playerIndex, int cellX, int cellY, GameObject smallObjectPrefab,GameObject largeObjectPrefab, ref GameObject largeObject, ref GameObject[] smallObject)
    {
        Vector3[] smallObjectPosition = new Vector3[2];
        Vector3 largeObjectPosition;
        Quaternion afterRotate = smallObjectPrefab.transform.rotation; //是否需要旋转180度

        smallObjectPosition[0] = new Vector3(cellX - 7f, smallObjectPrefab.transform.position.y, cellY + 0.5f);
        smallObjectPosition[1] = new Vector3(cellX - 7f, smallObjectPrefab.transform.position.y, cellY + 0.5f + ConstantParameter.distance_P1_P2);
        largeObjectPosition = new Vector3(smallObjectPosition[0].x * ConstantParameter.times_GroundToBoard, largeObjectPrefab.transform.position.y, smallObjectPosition[0].z * ConstantParameter.times_GroundToBoard + ConstantParameter.battleGroundZ);

        //P2 是奇数索引
        //改变转向
        if (playerIndex / 2 != (playerIndex + 1) / 2)
        {
            afterRotate = smallObjectPrefab.transform.rotation;
            afterRotate.eulerAngles += new Vector3(0f, 180f, 0f);
        }

        //实例化怪物，小的在棋盘上，大的在战场上
        smallObject[0] = UnityEngine.Object.Instantiate(smallObjectPrefab, smallObjectPosition[0], afterRotate);
        smallObject[1] = UnityEngine.Object.Instantiate(smallObjectPrefab, smallObjectPosition[1], afterRotate);
        largeObject = UnityEngine.Object.Instantiate(largeObjectPrefab, largeObjectPosition, afterRotate);

        //将怪物放到父物体上
        smallObject[0].transform.parent = StaticGameObject.checkerboard[0].transform;
        smallObject[1].transform.parent = StaticGameObject.checkerboard[1].transform;
        largeObject.transform.parent = StaticGameObject.battleGround.transform;
    }

    public void MoveCellObjectInfo(CellPosition origin, CellPosition target)
    {
        AddCellObjectInfo(CellParameter.CellInformation[origin.X, origin.Z].PlayerIndex, target.X, target.Z, CellParameter.CellInformation[origin.X, origin.Z].Name, CellParameter.CellInformation[origin.X, origin.Z].ObjectInCell_S, CellParameter.CellInformation[origin.X, origin.Z].ObjectInCell_L, CellParameter.CellInformation[origin.X, origin.Z].ObjectProperty);

        ClearCellObjectInfo(origin.X, origin.Z);
    }

    public void ClearCellObjectInfo(int cellX, int cellY)
    {
        int playerIndex = CellParameter.CellInformation[cellX, cellY].PlayerIndex;

        string monsterName = CellParameter.CellInformation[cellX, cellY].Name;
        //清理playerparameter
        MonsterData medium = PlayerParameter.Player[playerIndex].MonsterDict[monsterName];
        medium.Num--;
        for (int i = medium.MonsterCellIndexList.Count - 1; i >= 0; i--)
        {
            CellPosition element = medium.MonsterCellIndexList[i];
            if (element.X == cellX && element.Z == cellY)
            {
                medium.MonsterCellIndexList.Remove(element);
                break;
            }
        }

        PlayerParameter.Player[playerIndex].MonsterDict[monsterName] = medium;

        //清理cellparameter
        CellParameter.CellInformation[cellX, cellY].Name = ConstantParameter.EMPTYCELL;
        CellParameter.CellInformation[cellX, cellY].PlayerIndex = -1;
        CellParameter.CellInformation[cellX, cellY].ObjectInCell_S = null;
        CellParameter.CellInformation[cellX, cellY].ObjectInCell_L = null;
        CellParameter.CellInformation[cellX, cellY].ObjectProperty = null;
    }

    public void AddCellObjectInfo(int playerIndex, int cellX, int cellY, string monster_S_Name, GameObject[] smallObject, GameObject largeObject, dynamic objectProperty)
    {
        //更改playerparameter
        MonsterData medium = PlayerParameter.Player[playerIndex].MonsterDict[monster_S_Name];
        medium.Num++;
        medium.MonsterCellIndexList.Add(new CellPosition(cellX, cellY));
        PlayerParameter.Player[playerIndex].MonsterDict[monster_S_Name] = medium;

        //更改cellparameter
        //CellParameter.CellInformation[cellX, cellY].Name = monster_S_Name;
        //CellParameter.CellInformation[cellX, cellY].PlayerIndex = playerIndex;
        //CellParameter.CellInformation[cellX, cellY].ObjectInCell_S = smallObject;
        //CellParameter.CellInformation[cellX, cellY].ObjectInCell_L = largeObject;
        //CellParameter.CellInformation[cellX, cellY].ObjectProperty = objectProperty;
        AddCellParameterInfo(playerIndex, cellX, cellY, monster_S_Name, smallObject, largeObject, objectProperty);
    }

    public void AddCellParameterInfo(int playerIndex, int cellX, int cellY, string monster_S_Name, GameObject[] smallObject, GameObject largeObject, dynamic objectProperty)
    {
        CellParameter.CellInformation[cellX, cellY].Name = monster_S_Name;
        CellParameter.CellInformation[cellX, cellY].PlayerIndex = playerIndex;
        CellParameter.CellInformation[cellX, cellY].ObjectInCell_S = smallObject;
        CellParameter.CellInformation[cellX, cellY].ObjectInCell_L = largeObject;
        CellParameter.CellInformation[cellX, cellY].ObjectProperty = objectProperty;
    }


    //点击获得所点单元格
    //是否点击了并点击了正确的蒙版，点击返回true
    public bool ClickCellPosition(out CellPosition cellClicked)
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool playerIndexOdd;

            //LayerMask checkerboardLayer = LayerMask.GetMask("Checkerboard");        //获得射线碰撞的层
            RaycastHit checkerboard;            //射线射到的棋盘

            int x, y;

            if (PlayerParameter.ActivePlayerIndex / 2 == (PlayerParameter.ActivePlayerIndex + 1) / 2)
            {
                playerIndexOdd = false;
            }
            //如果是P2
            else
            {
                playerIndexOdd = true;
            }

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
                cellClicked = new CellPosition(x, y);

                return true;
            }
        }

        cellClicked = new CellPosition(-1, -1);

        return false;
    }
}