using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathFunction
{
    private CellFunction cellFunction = new CellFunction();

    //传送怪物
    public void Teleport(CellPosition origin, CellPosition target, GameObject gameObject)
    {
        GameObject[] smallAndLargeObject = new GameObject[3];

        //将怪物放进变量
        smallAndLargeObject[0] = CellParameter.CellInformation[origin.X, origin.Z].ObjectInCell_S[0];
        smallAndLargeObject[1] = CellParameter.CellInformation[origin.X, origin.Z].ObjectInCell_S[1];
        smallAndLargeObject[2] = CellParameter.CellInformation[origin.X, origin.Z].ObjectInCell_L;

        Vector3[] distance = new Vector3[3];

        distance[0] = new Vector3(target.X - origin.X, 0, target.Z - origin.Z);
        distance[1] = distance[0];
        distance[2] = distance[0] * ConstantParameter.times_GroundToBoard;

        for (int i = 0; i < 2; i++)
        {
            smallAndLargeObject[i].transform.position += distance[i];
        }

        cellFunction.MoveCellObjectInfo(origin, target);

        StaticGameObject.UIDiceParentObject.SetActive(true);

        UnityEngine.Object.Destroy(gameObject);
    }

    //让怪物移动
    public IEnumerator MonsterMove(int cellX, int cellY, List<CellPosition> path, GameObject gameObject)
    {
        GameObject[] smallAndLargeObject = new GameObject[3];
        Slider[] hpSliders = new Slider[2];

        //将怪物放进变量
        smallAndLargeObject[0] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[0];
        smallAndLargeObject[1] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[1];
        smallAndLargeObject[2] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_L;

        hpSliders = CellParameter.CellInformation[cellX, cellY].ObjectProperty.HpSlider;


        //根据下一个的位置
        for (int i = path.Count - 1; i > 0; i--)
        {
            //转方向
            Quaternion afterRotate = new Quaternion();

            //上边
            if (path[i].Z - path[i - 1].Z == -1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            //下边
            else if (path[i].Z - path[i - 1].Z == 1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            //左边
            else if (path[i].X - path[i - 1].X == 1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 270f, 0f);
            }
            //右边
            else if (path[i].X - path[i - 1].X == -1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 90f, 0f);
            }

            for (int j = 0; j < 3; j++)
            {
                smallAndLargeObject[j].transform.rotation = afterRotate;
            }

            //移动
            for (int j = 0; j < ConstantParameter.moveTime; j++)
            {
                //使血条对准摄像机
                for (int k = 0; k < 2; k++)
                {
                    hpSliders[k].transform.rotation = Quaternion.LookRotation(ConstantParameter.PlayerCameraPosition[k] - new Vector3(0f, hpSliders[k].transform.position.y, hpSliders[k].transform.position.z)); //= new Vector3(0f, 180f, 0f);

                    //再翻转180度
                    hpSliders[k].transform.eulerAngles += new Vector3(0f, 0f, 180f);
                }

                smallAndLargeObject[0].transform.position += smallAndLargeObject[0].transform.forward * ConstantParameter.diceHeight / ConstantParameter.moveTime;
                smallAndLargeObject[1].transform.position += smallAndLargeObject[1].transform.forward * ConstantParameter.diceHeight / ConstantParameter.moveTime;
                smallAndLargeObject[2].transform.position += smallAndLargeObject[2].transform.forward * ConstantParameter.diceHeight * ConstantParameter.times_GroundToBoard / ConstantParameter.moveTime;
                yield return new WaitForFixedUpdate();
            }

            //未采用每一格都有可能触发陷阱，因为路径的选择是A*不是玩家自己选择
        }

        cellFunction.MoveCellObjectInfo(new CellPosition(cellX, cellY), path[0]);

        //检测陷阱并爆发
        for (int j = 0; j < PlayerParameter.PlayerNum; j++)
        {
            if (j != CellParameter.CellInformation[cellX, cellY].PlayerIndex && FindIn(path[0], CellParameter.TrapsPositionList[j]))
            {
                //cellFunction.MoveCellObjectInfo(new CellPosition(cellX, cellY), path[0]);

                CellParameter.TrapsProperty[path[0].X, path[0].Z].Explode(path[0].X, path[0].Z);

                break;
            }
        }

        //重新打开骰子的ui
        StaticGameObject.UIDiceParentObject.SetActive(true);

        UnityEngine.Object.Destroy(gameObject);
    }


    //找到所有的可能性格子
    public List<CellPosition> FloodPath(CellPosition start, int distance, bool directDistanceMode = false, bool attackMode = false)
    {
        List<CellPosition> nowList = new List<CellPosition>();          //最外圈节点
        List<CellPosition> openList = new List<CellPosition>();         //最外圈的节点的所有下一趟最外圈
        List<CellPosition> closedList = new List<CellPosition>();       //里面的节点

        List<CellPosition> moveList = new List<CellPosition>();

        nowList.Add(start);

        //循环几次由可移动多少次决定
        for (int i = 0; i < distance; i++)
        {
            foreach (CellPosition current in nowList)
            {
                //测过的放进closed
                closedList.Add(current);

                List<CellPosition> neighbours = GetNeighbours(current);
                foreach (CellPosition neighbour in neighbours)
                {
                    //如果不为空 或者 已经在closed里了
                    if (directDistanceMode && FindIn(neighbour, closedList))
                    {
                        continue;
                    }

                    else if (!directDistanceMode && (CellParameter.CellInformation[neighbour.X, neighbour.Z].Name != ConstantParameter.EMPTYCELL || FindIn(neighbour, closedList)))
                    {
                        continue;
                    }

                    //如果不是已经计算过的点就加入到list里
                    if (!FindIn(neighbour, openList))
                    {
                        openList.Add(neighbour);

                        if (directDistanceMode)
                            if (attackMode && CellParameter.CellInformation[neighbour.X, neighbour.Z].PlayerIndex != -1)    //-1可以避免对障碍物发起攻击
                                moveList.Add(neighbour);
                            else
                            {
                                if (CellParameter.CellInformation[neighbour.X, neighbour.Z].Name == ConstantParameter.EMPTYCELL)
                                    moveList.Add(neighbour);
                            }

                        else if (!directDistanceMode)
                            moveList.Add(neighbour);
                    }
                }
            }

            //清理now和open
            nowList.Clear();
            //把所有的item加入到下一趟里
            foreach (CellPosition item in openList)
            {
                nowList.Add(item);
            }
            openList.Clear();

        }

        return moveList;
    }

    //寻找路径
    public List<CellPosition> AStarPath(CellPosition start, CellPosition end)
    {
        List<CellPosition> openList = new List<CellPosition>();     //最外圈的节点
        List<CellPosition> closedList = new List<CellPosition>();   //里面的节点

        //加入第一个节点
        openList.Add(start);

        //直到找到节点结束
        while (true)
        {
            //找到fcost最小的作为当前要测试的节点
            CellPosition current = LowestFCost(openList);
            openList.Remove(current);
            closedList.Add(current);

            //如果当前就是结束节点
            if (current.X == end.X && current.Z == end.Z)
            {
                List<CellPosition> path = PathFromEnd(current, start);

                //结束
                return path;
            }

            //对每一个邻居做测试
            List<CellPosition> neighbours = GetNeighbours(current);
            foreach (CellPosition neighbour in neighbours)
            {
                //如果不为空 或者 已经在closed里了
                if (CellParameter.CellInformation[neighbour.X, neighbour.Z].Name != ConstantParameter.EMPTYCELL || FindIn(neighbour, closedList))
                {
                    continue;
                }
                //如果不是已经计算过的点就计算完放进openlist里
                if (!FindIn(neighbour, openList))
                {
                    neighbour.CalculateCosts(start, end);
                    neighbour.Parent = current;
                    openList.Add(neighbour);
                }
            }
        }

    }

    //上下左右的邻居
    private List<CellPosition> GetNeighbours(CellPosition start)
    {
        List<CellPosition> neighbours = new List<CellPosition>();

        //上边
        if (start.Z != 14)
        {
            neighbours.Add(new CellPosition(start.X, start.Z + 1));
        }
        //下边
        if (start.Z != 0)
        {
            neighbours.Add(new CellPosition(start.X, start.Z - 1));
        }
        //左边
        if (start.X != 0)
        {
            neighbours.Add(new CellPosition(start.X - 1, start.Z));
        }
        //右边
        if (start.X != 14)
        {
            neighbours.Add(new CellPosition(start.X + 1, start.Z));
        }

        return neighbours;
    }

    public bool FindIn(CellPosition tested, List<CellPosition> cellPositionList)
    {
        foreach (CellPosition cell in cellPositionList)
        {
            if (cell.X == tested.X && cell.Z == tested.Z)
            {
                return true;
            }
        }

        return false;
    }

    //A*算法中获取list中最小代价的节点
    private CellPosition LowestFCost(List<CellPosition> cells)
    {
        CellPosition output = cells[0];

        foreach (CellPosition cell in cells)
        {
            if (cell.FCost < output.FCost)
            {
                output = cell;
            }
        }

        return output;
    }

    //A*算法根据最后节点的parent找到这条路径
    private List<CellPosition> PathFromEnd(CellPosition target, CellPosition start)
    {
        List<CellPosition> findPath = new List<CellPosition>();

        CellPosition current = target;

        while (current != start)
        {
            findPath.Add(current);

            current = current.Parent;
        }

        findPath.Add(current);

        return findPath;
    }
}
