using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//获得一次二倍攻击距离，并且不会受到反击
public class Point_333 : MonoBehaviour
{
    private int cellX, cellY;

    private List<CellPosition> attackable; //可攻击到的格子

    private CellPosition originCell;    //骰子的位置
    private CellPosition targetCell;    //目标的格子


    private bool flag_Attackable = false;//减少消耗 可以不进入update里的其他代码第二次

    private PathFunction pathFunction = new PathFunction();
    private CellFunction cellFunction = new CellFunction();
    private AttackFunction attackFunction = new AttackFunction();

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -ConstantParameter.diceHeight / 2f && !flag_Attackable)
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
            //不是个能攻击的怪
            if (CellParameter.CellInformation[cellX, cellY].PlayerIndex != PlayerParameter.ActivePlayerIndex || !CellParameter.CellInformation[cellX, cellY].ObjectProperty.Attackability)
            {
                Destroy(gameObject);
                return;
            }

            //计算可以攻击到的格子
            originCell = new CellPosition(cellX, cellY);
            attackable = pathFunction.FloodPath(originCell, CellParameter.CellInformation[originCell.X, originCell.Z].ObjectProperty.BasicAttackDistance * 2, true, true);

            if (!attackFunction.FindEnemy(attackable, PlayerParameter.ActivePlayerIndex))
            {
                Destroy(gameObject);
                return;
            }
            
            //关掉UI骰子方便选择格子
            StaticGameObject.UIDiceParentObject.SetActive(false);
            flag_Attackable = true;

            //展示可攻击到的格子
        }

        //点击格子
        if (flag_Attackable && cellFunction.ClickCellPosition(out targetCell))
        {
            if (//CellParameter.CellInformation[targetCell.X, targetCell.Z].Name != ConstantParameter.EMPTYCELL
                CellParameter.CellInformation[targetCell.X, targetCell.Z].PlayerIndex != -1
                && CellParameter.CellInformation[targetCell.X, targetCell.Z].PlayerIndex != PlayerParameter.ActivePlayerIndex
                && pathFunction.FindIn(targetCell, attackable)
                )
            {
                attackFunction.NonCounterBasicAttack(originCell.X, originCell.Z, targetCell.X, targetCell.Z, gameObject);
            }
        }
    }
}
