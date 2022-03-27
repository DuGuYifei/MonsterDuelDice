using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_2 : MonoBehaviour
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
            movable = pathFunction.FloodPath(originCell,
                CellParameter.CellInformation[cellX, cellY].ObjectProperty.Mobility
                //15
                );

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
                path = pathFunction.AStarPath(originCell, targetCell);

                StartCoroutine(pathFunction.MonsterMove(cellX, cellY, path, gameObject));
            }
        }
    }

    ////让怪物移动
    //IEnumerator MonsterMove()
    //{
    //    GameObject[] smallAndLargeObject = new GameObject[3];

    //    //将怪物放进变量
    //    smallAndLargeObject[0] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[0];
    //    smallAndLargeObject[1] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[1];
    //    smallAndLargeObject[2] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_L;

    //    //根据下一个的位置
    //    for (int i = path.Count - 1; i > 0; i--)
    //    {
    //        //转方向
    //        Quaternion afterRotate = new Quaternion();

    //        //上边
    //        if (path[i].Z - path[i - 1].Z == -1)
    //        {
    //            afterRotate.eulerAngles = new Vector3(0f, 0f, 0f);
    //        }
    //        //下边
    //        else if (path[i].Z - path[i - 1].Z == 1)
    //        {
    //            afterRotate.eulerAngles = new Vector3(0f, 180f, 0f);
    //        }
    //        //左边
    //        else if (path[i].X - path[i - 1].X == 1)
    //        {
    //            afterRotate.eulerAngles = new Vector3(0f, 270f, 0f);
    //        }
    //        //右边
    //        else if (path[i].X - path[i - 1].X == -1)
    //        {
    //            afterRotate.eulerAngles = new Vector3(0f, 90f, 0f);
    //        }

    //        for(int j = 0;j<3;j++)
    //        {
    //            smallAndLargeObject[j].transform.rotation = afterRotate;
    //        }

    //        //移动
    //        for (int j = 0; j < ConstantParameter.moveTime; j++)
    //        {

    //            smallAndLargeObject[0].transform.position += smallAndLargeObject[0].transform.forward * ConstantParameter.diceHeight / ConstantParameter.moveTime;
    //            smallAndLargeObject[1].transform.position += smallAndLargeObject[1].transform.forward * ConstantParameter.diceHeight / ConstantParameter.moveTime;
    //            smallAndLargeObject[2].transform.position += smallAndLargeObject[2].transform.forward * ConstantParameter.diceHeight * ConstantParameter.times_GroundToBoard / ConstantParameter.moveTime;
    //            yield return new WaitForFixedUpdate();
    //        }
    //    }

    //    //重新打开骰字的ui
    //    StaticGameObject.UIDiceParentObject.SetActive(true);

    //    Destroy(gameObject);
    //}


    ////找到所有的可能性格子
    //private List<CellPosition> FloodPath(CellPosition start, int distance)
    //{
    //    List<CellPosition> nowList = new List<CellPosition>();          //最外圈节点
    //    List<CellPosition> openList = new List<CellPosition>();         //最外圈的节点的所有下一趟最外圈
    //    List<CellPosition> closedList = new List<CellPosition>();       //里面的节点

    //    List<CellPosition> moveList = new List<CellPosition>();

    //    nowList.Add(start);

    //    //循环几次由可移动多少次决定
    //    for (int i = 0; i < distance; i++)
    //    {
    //        foreach (CellPosition current in nowList)
    //        {
    //            //测过的放进closed
    //            closedList.Add(current);

    //            List<CellPosition> neighbours = GetNeighbours(current);
    //            foreach (CellPosition neighbour in neighbours)
    //            {
    //                //如果不为空 或者 已经在closed里了
    //                if (CellParameter.CellInformation[neighbour.X, neighbour.Z].Name != ConstantParameter.EMPTYCELL || FindIn(neighbour, closedList))
    //                {
    //                    continue;
    //                }

    //                //如果不是已经计算过的点就加入到list里
    //                if (!FindIn(neighbour, openList))
    //                {
    //                    openList.Add(neighbour);
    //                    moveList.Add(neighbour);
    //                }
    //            }
    //        }

    //        //清理now和open
    //        nowList.Clear();
    //        //把所有的item加入到下一趟里
    //        foreach (CellPosition item in openList)
    //        {
    //            nowList.Add(item);
    //        }
    //        openList.Clear();

    //    }

    //    return moveList;
    //}

    ////寻找路径
    //private CellPosition AStarPath(CellPosition start, CellPosition end)
    //{
    //    List<CellPosition> openList = new List<CellPosition>();     //最外圈的节点
    //    List<CellPosition> closedList = new List<CellPosition>();   //里面的节点

    //    //加入第一个节点
    //    openList.Add(start);

    //    //直到找到节点结束
    //    while (true)
    //    {
    //        //找到fcost最小的作为当前要测试的节点
    //        CellPosition current = LowestFCost(openList);
    //        openList.Remove(current);
    //        closedList.Add(current);

    //        //如果当前就是结束节点
    //        if (current.X == end.X && current.Z == end.Z)
    //        {
    //            path = PathFromEnd(current, start);

    //            //结束
    //            return current;
    //        }

    //        //对每一个邻居做测试
    //        List<CellPosition> neighbours = GetNeighbours(current);
    //        foreach (CellPosition neighbour in neighbours)
    //        {
    //            //如果不为空 或者 已经在closed里了
    //            if (CellParameter.CellInformation[neighbour.X, neighbour.Z].Name != ConstantParameter.EMPTYCELL || FindIn(neighbour, closedList))
    //            {
    //                continue;
    //            }
    //            //如果不是已经计算过的点就计算完放进openlist里
    //            if (!FindIn(neighbour, openList))
    //            {
    //                neighbour.CalculateCosts(start, end);
    //                neighbour.Parent = current;
    //                openList.Add(neighbour);
    //            }
    //        }
    //    }

    //}

    ////上下左右的邻居
    //private List<CellPosition> GetNeighbours(CellPosition start)
    //{
    //    List<CellPosition> neighbours = new List<CellPosition>();

    //    //上边
    //    if (start.Z != 14)
    //    {
    //        neighbours.Add(new CellPosition(start.X, start.Z + 1));
    //    }
    //    //下边
    //    if (start.Z != 0)
    //    {
    //        neighbours.Add(new CellPosition(start.X, start.Z - 1));
    //    }
    //    //左边
    //    if (start.X != 0)
    //    {
    //        neighbours.Add(new CellPosition(start.X - 1, start.Z));
    //    }
    //    //右边
    //    if (start.X != 14)
    //    {
    //        neighbours.Add(new CellPosition(start.X + 1, start.Z));
    //    }

    //    return neighbours;
    //}

    //private bool FindIn(CellPosition tested, List<CellPosition> cellPositionList)
    //{
    //    foreach (CellPosition cell in cellPositionList)
    //    {
    //        if (cell.X == tested.X && cell.Z == tested.Z)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    ////A*算法中获取list中最小代价的节点
    //private CellPosition LowestFCost(List<CellPosition> cells)
    //{
    //    CellPosition output = cells[0];

    //    foreach (CellPosition cell in cells)
    //    {
    //        if (cell.FCost <= output.FCost)
    //        {
    //            output = cell;
    //        }
    //    }

    //    return output;
    //}

    ////A*算法根据最后节点的parent找到这条路径
    //private List<CellPosition> PathFromEnd(CellPosition target, CellPosition start)
    //{
    //    List<CellPosition> findPath = new List<CellPosition>();

    //    CellPosition current = target;

    //    while (current != start)
    //    {
    //        findPath.Add(current);

    //        current = current.Parent;
    //    }

    //    findPath.Add(current);

    //    return findPath;
    //}
}
