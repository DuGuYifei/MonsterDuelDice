using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_222 : MonoBehaviour
{
    private int cellX, cellY;

    private List<CellPosition> movable; //可移动到的格子
    private List<CellPosition> path;    //移动的路径

    private CellPosition originCell;    //起点
    private CellPosition targetCell;    //目标的终点格子

    private bool flag_StartMove = false;//减少消耗 可以不进入update里的其他代码第二次

    private PathFunction pathFunction = new PathFunction();
    private CellFunction cellFunction = new CellFunction();

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -ConstantParameter.diceHeight / 2f && !flag_StartMove)
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
            //不是个能移动的怪
            if (CellParameter.CellInformation[cellX, cellY].PlayerIndex != PlayerParameter.ActivePlayerIndex || CellParameter.CellInformation[cellX, cellY].ObjectProperty.Mobility <= 0)
            {
                Destroy(gameObject);
                return;
            }

            //计算可以移动到的格子
            originCell = new CellPosition(cellX, cellY);
            movable = pathFunction.FloodPath(originCell, ConstantParameter.moveDistane_Point222, true);

            //防止没有可移动的格子
            if (movable.Count == 0)
            {
                Destroy(gameObject);
                return;
            }

            //关掉UI骰子方便选择格子
            StaticGameObject.UIDiceParentObject.SetActive(false);
            flag_StartMove = true;
            //展示可移动到的格子
        }

        //点击格子
        if (flag_StartMove && cellFunction.ClickCellPosition(out targetCell))
        {
            if (pathFunction.FindIn(targetCell, movable))
            {
                pathFunction.Teleport(originCell, targetCell, gameObject);
            }
        }
    }
}
