using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapIndex
{
    point5,
    point55
}

public class TrapCellFunction
{
    //毁掉旧的骰子
    public void DestroyTrapObject(int cellX, int cellZ)
    {
        int playerIndex = CellParameter.TrapsProperty[cellX, cellZ].PlayerIndex;
        UnityEngine.Object.Destroy(CellParameter.TrapsProperty[cellX, cellZ].Trap_S[0]);
        UnityEngine.Object.Destroy(CellParameter.TrapsProperty[cellX, cellZ].Trap_S[1]);
        UnityEngine.Object.Destroy(CellParameter.TrapsProperty[cellX, cellZ].Trap_L);

        ClearTrapObjectInfo(cellX, cellZ);
    }

    //新的陷阱，0是骰子5，1是骰子55
    public void NewTrapObject(int playerIndex, int cellX, int cellZ, TrapIndex trapIndex)
    {
        GameObject[] smallTrap = new GameObject[2];
        GameObject smallTrapPrefab = PlayerParameter.Player[playerIndex].TrapsData.TrapPrefab_S[(int)trapIndex];
        GameObject largeTrap = PlayerParameter.Player[playerIndex].TrapsData.TrapPrefab_L[(int)trapIndex];
        Vector3[] smallTrapPosition = new Vector3[2];
        Vector3 largeTrapPosition;
        Quaternion afterRotate = smallTrapPrefab.transform.rotation; //是否需要旋转180度

        //召唤怪兽
        //P1
        smallTrapPosition[0] = new Vector3(cellX - 7f, smallTrapPrefab.transform.position.y, cellZ + 0.5f);
        smallTrapPosition[1] = new Vector3(cellX - 7f, smallTrapPrefab.transform.position.y, cellZ + 0.5f + ConstantParameter.distance_P1_P2);
        largeTrapPosition = new Vector3(smallTrapPosition[0].x * ConstantParameter.times_GroundToBoard, largeTrap.transform.position.y, smallTrapPosition[0].z * ConstantParameter.times_GroundToBoard + ConstantParameter.battleGroundZ);

        smallTrapPrefab.layer = 6;
        largeTrap.layer = 6;

        //P2 是奇数索引
        //改变转向
        if (playerIndex / 2 != (playerIndex + 1) / 2)
        {
            afterRotate = smallTrapPrefab.transform.rotation;
            afterRotate.eulerAngles += new Vector3(0f, 180f, 0f);
            smallTrapPrefab.layer = 7;
            largeTrap.layer = 7;
        }

        //实例化怪物，小的在棋盘上，大的在战场上
        smallTrap[0] = UnityEngine.Object.Instantiate(smallTrapPrefab, smallTrapPosition[0], afterRotate);
        smallTrap[1] = UnityEngine.Object.Instantiate(smallTrapPrefab, smallTrapPosition[1], afterRotate);
        largeTrap = UnityEngine.Object.Instantiate(largeTrap, largeTrapPosition, afterRotate);

        //将怪物放到父物体上
        smallTrap[0].transform.parent = StaticGameObject.checkerboard[0].transform;
        smallTrap[1].transform.parent = StaticGameObject.checkerboard[1].transform;
        largeTrap.transform.parent = StaticGameObject.battleGround.transform;

        Type t = Type.GetType(ObjectClassNameParameter.className[PlayerParameter.Player[playerIndex].TrapsData.TrapsName[(int)trapIndex]]);
        dynamic objectProperty = Activator.CreateInstance(t);
        objectProperty.PlayerIndex = playerIndex;

        objectProperty.Trap_S = smallTrap;
        objectProperty.Trap_L = largeTrap;

        AddTrapObjectInfo(playerIndex, cellX, cellZ, objectProperty);
    }

    public void AddTrapObjectInfo(int playerIndex, int cellX, int cellZ, dynamic trapProperty)
    {
        //陷阱的脚本
        CellParameter.TrapsProperty[cellX, cellZ] = trapProperty;
        //陷阱的位置
        CellParameter.TrapsPositionList[playerIndex].Add(new CellPosition(cellX, cellZ));
    }

    public void ClearTrapObjectInfo(int cellX, int cellZ)
    {
        int playerIndex = CellParameter.TrapsProperty[cellX, cellZ].PlayerIndex;

        CellParameter.TrapsProperty[cellX, cellZ] = null;

        foreach (CellPosition cell in CellParameter.TrapsPositionList[playerIndex])
        {
            if (cell.X == cellX && cell.Z == cellZ)
                CellParameter.TrapsPositionList[playerIndex].Remove(cell);
            break;
        }
    } 
}
