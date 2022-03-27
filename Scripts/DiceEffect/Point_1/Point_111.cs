using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_111 : MonoBehaviour
{
    private string newMonsterName;

    private int cellX, cellY;

    private CellFunction cellFunction = new CellFunction();

    void Start()
    {
        newMonsterName = PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -ConstantParameter.diceHeight / 2f)
        {
            //根据playerindex和骰字位置得到cellX和cellY的坐标
            cellX = (int)transform.position.x + 7;
            //如果是P1
            if (PlayerParameter.ActivePlayerIndex / 2 == (PlayerParameter.ActivePlayerIndex + 1) / 2)
            {
                cellY = (int)transform.position.z;
            }
            //如果是P2
            else
            {
                cellY = (int)transform.position.z - 495;
            }

            //diceheight 其实和格子边长一样长
            //if 骰子超出了倒数三行 或者 放置的格子里有东西
            if (!((transform.position.z > ConstantParameter.diceHeight * 3f && transform.position.z < ConstantParameter.distance_P1_P2 + ConstantParameter.diceHeight * 12f) || CellParameter.CellInformation[cellX, cellY].Name != ConstantParameter.EMPTYCELL))
            {
                cellFunction.NewCellObject(PlayerParameter.ActivePlayerIndex, cellX, cellY, newMonsterName);
            }
            Destroy(gameObject);
        }
    }
}
