using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_666 : MonoBehaviour
{
    private int cellX, cellY;

    private CellFunction cellFunction = new CellFunction();


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
                cellFunction.NewCellObject(PlayerParameter.ActivePlayerIndex, cellX, cellY, PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[HighestLevelMonsterInGraveyard() - 1]);
            }
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 返回墓地等级最高的怪，如果墓地为空则返回默认等级。
    /// </summary>
    /// <returns>等级最高的怪的等级</returns>
    private int HighestLevelMonsterInGraveyard()
    {
        int index = 4;
        for (; index >= 0; index--)
        {
            if (PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterGraveyardNum[index] > 0)
            {
                break;
            }
        }

        if (index == -1)
            return 3;
        else
            return index + 1;
    }
}
