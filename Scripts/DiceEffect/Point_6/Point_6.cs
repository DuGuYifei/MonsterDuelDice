using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_6 : MonoBehaviour
{
    private int cellX, cellY;
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -ConstantParameter.diceHeight / 2f)
        {
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

            //不是自己的怪
            //能加盾的怪都在player 的 monster name 变量里
            if (CellParameter.CellInformation[cellX, cellY].PlayerIndex != PlayerParameter.ActivePlayerIndex || !PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name.Contains(CellParameter.CellInformation[cellX, cellY].Name))
            {
                Destroy(gameObject);
                return;
            }

            CellParameter.CellInformation[cellX, cellY].ObjectProperty.Shield += CellParameter.CellInformation[cellX, cellY].ObjectProperty.Armor;

            //Debug.Log(cellX + "," + cellY + "护甲是" + CellParameter.CellInformation[cellX, cellY].ObjectProperty.Shield);

            Destroy(gameObject);
        }
    }
}
