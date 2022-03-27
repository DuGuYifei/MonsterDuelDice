using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点数66：
/// 驱散负面效果
/// </summary>
public class Point_66 : MonoBehaviour
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
            //能加负面效果的怪都在 player 的 monster name 变量里
            if (CellParameter.CellInformation[cellX, cellY].PlayerIndex != PlayerParameter.ActivePlayerIndex || !PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name.Contains(CellParameter.CellInformation[cellX, cellY].Name))
            {
                Destroy(gameObject);
                return;
            }

            //foreach(var element in CellParameter.CellInformation[cellX,cellY].ObjectProperty.DeBuff)
            //{
            //    //如果有不可驱散效果可以加一些if
            //    CellParameter.CellInformation[cellX, cellY].ObjectProperty.DeBuff[element.key] = 0;
            //}

            List<string> keys = new List<string>();
            foreach (string element in CellParameter.CellInformation[cellX, cellY].ObjectProperty.DeBuff.Keys)
            {
                keys.Add(element);
            }

            for (int i = 0; i < CellParameter.CellInformation[cellX, cellY].ObjectProperty.DeBuff.Count; i++)
            {

                    CellParameter.CellInformation[cellX, cellY].ObjectProperty.DeBuff[keys[i]] = 0;

                    //Debug.Log(keys[i] + "剩余回合" + CellParameter.CellInformation[cellX, cellY].ObjectProperty.DeBuff[keys[i]]);
                }


                Destroy(gameObject);
        }
    }
}
