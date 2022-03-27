using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_5 : MonoBehaviour
{
    private int cellX, cellY;

    private TrapCellFunction trapCellFunction = new TrapCellFunction();

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
            //if 骰子超出了倒数七行 或者 放置的格子里有东西
            if (!((transform.position.z > ConstantParameter.diceHeight * 7f && transform.position.z < ConstantParameter.distance_P1_P2 + ConstantParameter.diceHeight * 8f) || CellParameter.CellInformation[cellX, cellY].Name != ConstantParameter.EMPTYCELL))
            {
                trapCellFunction.NewTrapObject(PlayerParameter.ActivePlayerIndex, cellX, cellY, TrapIndex.point5);
            }

            Destroy(gameObject);
        }
    }
}
