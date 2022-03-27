using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathFunction
{
    private CellFunction cellFunction = new CellFunction();

    //���͹���
    public void Teleport(CellPosition origin, CellPosition target, GameObject gameObject)
    {
        GameObject[] smallAndLargeObject = new GameObject[3];

        //������Ž�����
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

    //�ù����ƶ�
    public IEnumerator MonsterMove(int cellX, int cellY, List<CellPosition> path, GameObject gameObject)
    {
        GameObject[] smallAndLargeObject = new GameObject[3];
        Slider[] hpSliders = new Slider[2];

        //������Ž�����
        smallAndLargeObject[0] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[0];
        smallAndLargeObject[1] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_S[1];
        smallAndLargeObject[2] = CellParameter.CellInformation[cellX, cellY].ObjectInCell_L;

        hpSliders = CellParameter.CellInformation[cellX, cellY].ObjectProperty.HpSlider;


        //������һ����λ��
        for (int i = path.Count - 1; i > 0; i--)
        {
            //ת����
            Quaternion afterRotate = new Quaternion();

            //�ϱ�
            if (path[i].Z - path[i - 1].Z == -1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            //�±�
            else if (path[i].Z - path[i - 1].Z == 1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            //���
            else if (path[i].X - path[i - 1].X == 1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 270f, 0f);
            }
            //�ұ�
            else if (path[i].X - path[i - 1].X == -1)
            {
                afterRotate.eulerAngles = new Vector3(0f, 90f, 0f);
            }

            for (int j = 0; j < 3; j++)
            {
                smallAndLargeObject[j].transform.rotation = afterRotate;
            }

            //�ƶ�
            for (int j = 0; j < ConstantParameter.moveTime; j++)
            {
                //ʹѪ����׼�����
                for (int k = 0; k < 2; k++)
                {
                    hpSliders[k].transform.rotation = Quaternion.LookRotation(ConstantParameter.PlayerCameraPosition[k] - new Vector3(0f, hpSliders[k].transform.position.y, hpSliders[k].transform.position.z)); //= new Vector3(0f, 180f, 0f);

                    //�ٷ�ת180��
                    hpSliders[k].transform.eulerAngles += new Vector3(0f, 0f, 180f);
                }

                smallAndLargeObject[0].transform.position += smallAndLargeObject[0].transform.forward * ConstantParameter.diceHeight / ConstantParameter.moveTime;
                smallAndLargeObject[1].transform.position += smallAndLargeObject[1].transform.forward * ConstantParameter.diceHeight / ConstantParameter.moveTime;
                smallAndLargeObject[2].transform.position += smallAndLargeObject[2].transform.forward * ConstantParameter.diceHeight * ConstantParameter.times_GroundToBoard / ConstantParameter.moveTime;
                yield return new WaitForFixedUpdate();
            }

            //δ����ÿһ���п��ܴ������壬��Ϊ·����ѡ����A*��������Լ�ѡ��
        }

        cellFunction.MoveCellObjectInfo(new CellPosition(cellX, cellY), path[0]);

        //������岢����
        for (int j = 0; j < PlayerParameter.PlayerNum; j++)
        {
            if (j != CellParameter.CellInformation[cellX, cellY].PlayerIndex && FindIn(path[0], CellParameter.TrapsPositionList[j]))
            {
                //cellFunction.MoveCellObjectInfo(new CellPosition(cellX, cellY), path[0]);

                CellParameter.TrapsProperty[path[0].X, path[0].Z].Explode(path[0].X, path[0].Z);

                break;
            }
        }

        //���´����ӵ�ui
        StaticGameObject.UIDiceParentObject.SetActive(true);

        UnityEngine.Object.Destroy(gameObject);
    }


    //�ҵ����еĿ����Ը���
    public List<CellPosition> FloodPath(CellPosition start, int distance, bool directDistanceMode = false, bool attackMode = false)
    {
        List<CellPosition> nowList = new List<CellPosition>();          //����Ȧ�ڵ�
        List<CellPosition> openList = new List<CellPosition>();         //����Ȧ�Ľڵ��������һ������Ȧ
        List<CellPosition> closedList = new List<CellPosition>();       //����Ľڵ�

        List<CellPosition> moveList = new List<CellPosition>();

        nowList.Add(start);

        //ѭ�������ɿ��ƶ����ٴξ���
        for (int i = 0; i < distance; i++)
        {
            foreach (CellPosition current in nowList)
            {
                //����ķŽ�closed
                closedList.Add(current);

                List<CellPosition> neighbours = GetNeighbours(current);
                foreach (CellPosition neighbour in neighbours)
                {
                    //�����Ϊ�� ���� �Ѿ���closed����
                    if (directDistanceMode && FindIn(neighbour, closedList))
                    {
                        continue;
                    }

                    else if (!directDistanceMode && (CellParameter.CellInformation[neighbour.X, neighbour.Z].Name != ConstantParameter.EMPTYCELL || FindIn(neighbour, closedList)))
                    {
                        continue;
                    }

                    //��������Ѿ�������ĵ�ͼ��뵽list��
                    if (!FindIn(neighbour, openList))
                    {
                        openList.Add(neighbour);

                        if (directDistanceMode)
                            if (attackMode && CellParameter.CellInformation[neighbour.X, neighbour.Z].PlayerIndex != -1)    //-1���Ա�����ϰ��﷢�𹥻�
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

            //����now��open
            nowList.Clear();
            //�����е�item���뵽��һ����
            foreach (CellPosition item in openList)
            {
                nowList.Add(item);
            }
            openList.Clear();

        }

        return moveList;
    }

    //Ѱ��·��
    public List<CellPosition> AStarPath(CellPosition start, CellPosition end)
    {
        List<CellPosition> openList = new List<CellPosition>();     //����Ȧ�Ľڵ�
        List<CellPosition> closedList = new List<CellPosition>();   //����Ľڵ�

        //�����һ���ڵ�
        openList.Add(start);

        //ֱ���ҵ��ڵ����
        while (true)
        {
            //�ҵ�fcost��С����Ϊ��ǰҪ���ԵĽڵ�
            CellPosition current = LowestFCost(openList);
            openList.Remove(current);
            closedList.Add(current);

            //�����ǰ���ǽ����ڵ�
            if (current.X == end.X && current.Z == end.Z)
            {
                List<CellPosition> path = PathFromEnd(current, start);

                //����
                return path;
            }

            //��ÿһ���ھ�������
            List<CellPosition> neighbours = GetNeighbours(current);
            foreach (CellPosition neighbour in neighbours)
            {
                //�����Ϊ�� ���� �Ѿ���closed����
                if (CellParameter.CellInformation[neighbour.X, neighbour.Z].Name != ConstantParameter.EMPTYCELL || FindIn(neighbour, closedList))
                {
                    continue;
                }
                //��������Ѿ�������ĵ�ͼ�����Ž�openlist��
                if (!FindIn(neighbour, openList))
                {
                    neighbour.CalculateCosts(start, end);
                    neighbour.Parent = current;
                    openList.Add(neighbour);
                }
            }
        }

    }

    //�������ҵ��ھ�
    private List<CellPosition> GetNeighbours(CellPosition start)
    {
        List<CellPosition> neighbours = new List<CellPosition>();

        //�ϱ�
        if (start.Z != 14)
        {
            neighbours.Add(new CellPosition(start.X, start.Z + 1));
        }
        //�±�
        if (start.Z != 0)
        {
            neighbours.Add(new CellPosition(start.X, start.Z - 1));
        }
        //���
        if (start.X != 0)
        {
            neighbours.Add(new CellPosition(start.X - 1, start.Z));
        }
        //�ұ�
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

    //A*�㷨�л�ȡlist����С���۵Ľڵ�
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

    //A*�㷨�������ڵ��parent�ҵ�����·��
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
