using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点数555：（地形魔法）
/// 可以让骰子展开出现障碍物
/// 不可以放在最后一行
/// </summary>
public class Point_555 : MonoBehaviour
{
    private GameObject[] NetCubeUICanvas;

    private int cellX, cellY;

    private bool flag_GenerateTopography = false;

    //private LayerMask NetCubeUILayer;
    private RaycastHit netCubeInUI;            //射线射到的棋盘

    private List<int> topographyXList;
    private List<int> topographyYList;

    private GameObject topographyObstaclePrefab;
    private GameObject topographyObstaclePrefab_L;
    private GameObject[] topographyObstacle;
    private GameObject topographyObstacle_L;

    private CellFunction cellFunction;

    private float frameNum = 120f;

    void Awake()
    {
        //NetCubeUILayer = LayerMask.GetMask("NetCubeUI");

        NetCubeUICanvas = new GameObject[PlayerParameter.PlayerNum];
        for (int i = 0; i < PlayerParameter.PlayerNum; i++)
        {
            NetCubeUICanvas[i] = StaticGameObject.checkerboard[i].transform.Find("PlayerControlUI_P" + (i + 1)).gameObject;
        }

        topographyObstaclePrefab = Resources.Load<GameObject>(ConstantParameter.TopographyObstaclePath);

        topographyObstacle = new GameObject[2];
        topographyObstaclePrefab_L = Resources.Load<GameObject>(ConstantParameter.TopographyObstaclePath + "_L");

        cellFunction = new CellFunction();
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag_GenerateTopography && transform.position.y <= -ConstantParameter.diceHeight / 2f)
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
            //if 骰子超出了倒数五行 或者在最下一行 或者 放置的格子里有东西
            if (!(transform.position.z< ConstantParameter.diceHeight * 1f|| transform.position.z > ConstantParameter.diceHeight * 14f + ConstantParameter.distance_P1_P2 || (transform.position.z > ConstantParameter.diceHeight * 5f && transform.position.z < ConstantParameter.distance_P1_P2 + ConstantParameter.diceHeight * 10f) || CellParameter.CellInformation[cellX, cellY].Name != ConstantParameter.EMPTYCELL))
            {
                flag_GenerateTopography = true;

                //关掉骰子UI选择
                StaticGameObject.UIDiceParentObject.SetActive(false);

                //开启选择立方体展开的UI
                NetCubeUICanvas[PlayerParameter.ActivePlayerIndex].SetActive(true);

                topographyXList = new List<int>();
                topographyYList = new List<int>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //可以开始执行555的效果了
        if (flag_GenerateTopography)
        {
            //如果右击,就旋转90度   
            if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.allCameras[1].ScreenPointToRay(Input.mousePosition), out netCubeInUI, 100, 1 << 8))
            {
                Quaternion afterRotate = netCubeInUI.collider.gameObject.transform.localRotation;
                afterRotate.eulerAngles += new Vector3(0f, 0f, 90f);
                //转了一圈就回到0
                if (afterRotate.eulerAngles.z == 360)
                    afterRotate.eulerAngles = new Vector3(0f, 0f, 0f);

                netCubeInUI.collider.gameObject.transform.localRotation = afterRotate;
            }

            //如果左击，根据名字生成合适的即将生成的障碍物组
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.allCameras[1].ScreenPointToRay(Input.mousePosition), out netCubeInUI, 100, 1 << 8))
            {
                GenerateTopographyPositionList();

                NetCubeUICanvas[PlayerParameter.ActivePlayerIndex].SetActive(false);

                StartCoroutine(GenerateTopography());
            }

            //如果点击中键，就让图片翻转180度
            if (Input.GetMouseButtonDown(2) && Physics.Raycast(Camera.allCameras[1].ScreenPointToRay(Input.mousePosition), out netCubeInUI, 100, 1 << 8))
            {
                Quaternion afterRotate = netCubeInUI.collider.gameObject.transform.localRotation;

                afterRotate.eulerAngles += new Vector3(0f, 180f, 0f);
                //转了一圈就回到0
                if (afterRotate.eulerAngles.y == 360f)
                    afterRotate.eulerAngles = new Vector3(0f, 0f, 0f);

                netCubeInUI.collider.gameObject.transform.localRotation = afterRotate;
            }
        }
    }

    //协程生成地形
    IEnumerator GenerateTopography()
    {
        List<GameObject> Obstacles = new List<GameObject>();
        List<GameObject> Obstacles_L = new List<GameObject>();
        for (int i = 0; i < topographyXList.Count; i++)
        {
            cellFunction.ObjectInstantiate(PlayerParameter.ActivePlayerIndex, topographyXList[i], topographyYList[i], topographyObstaclePrefab, topographyObstaclePrefab_L, ref topographyObstacle_L, ref topographyObstacle);

            Obstacles.Add(topographyObstacle[0]);
            Obstacles.Add(topographyObstacle[1]);
            Obstacles_L.Add(topographyObstacle_L);

            cellFunction.AddCellParameterInfo(-1, topographyXList[i], topographyYList[i], ConstantParameter.TOPOGRAPHYOBSTACLE, topographyObstacle, topographyObstacle_L, null);
        }

        Vector3 distancePerFrame = new Vector3(0f, topographyObstaclePrefab.transform.position.y * (-2) / frameNum, 0f);
        Vector3 distancePerFrame_L = new Vector3(0f, (topographyObstaclePrefab_L.transform.position.y - ConstantParameter.battleGroundY) * (-2) / frameNum, 0f);
        for (int frame = 0; frame < frameNum; frame++)
        {
            foreach (GameObject element in Obstacles)
            {
                element.transform.position += distancePerFrame;
            }

            foreach (GameObject element in Obstacles_L)
            {
                element.transform.position += distancePerFrame_L;
            }

            yield return new WaitForFixedUpdate();
        }


        StaticGameObject.UIDiceParentObject.SetActive(true);
        Destroy(gameObject);
    }

    //生成地形要在的坐标list
    private void GenerateTopographyPositionList()
    {
        int uiName = int.Parse(netCubeInUI.collider.gameObject.name.Substring(7));

        //先加入所有默认方向
        //底边(落骰子的地方)
        topographyXList.Add(cellX);
        topographyYList.Add(cellY);

        //P2
        int playerIndexOdd = -1;

        //P1
        if (PlayerParameter.ActivePlayerIndex / 2 == (PlayerParameter.ActivePlayerIndex + 1) / 2)
        {
            playerIndexOdd = 1;
        }

        //右边
        topographyXList.Add(cellX + playerIndexOdd * 1);
        topographyYList.Add(cellY);

        if (uiName <= 10)
        {
            //右边的右边
            topographyXList.Add(cellX + playerIndexOdd * 2);
            topographyYList.Add(cellY);

            if (uiName <= 6)
            {
                //左边
                topographyXList.Add(cellX - playerIndexOdd * 1);
                topographyYList.Add(cellY);

                //上下的Y
                topographyYList.Add(cellY + playerIndexOdd * 1);
                topographyYList.Add(cellY - playerIndexOdd * 1);

                if (uiName <= 4)
                {
                    //上面的在第一个
                    topographyXList.Add(cellX - playerIndexOdd * 1);

                    if (uiName == 1)
                        topographyXList.Add(cellX - playerIndexOdd * 1);

                    else if (uiName == 2)
                        topographyXList.Add(cellX);

                    else if (uiName == 3)
                        topographyXList.Add(cellX + playerIndexOdd * 1);

                    else if (uiName == 4)
                        topographyXList.Add(cellX + playerIndexOdd * 2);
                }

                else
                {
                    //上面的在第二个
                    topographyXList.Add(cellX);

                    if (uiName == 5)
                        topographyXList.Add(cellX);
                    else
                        topographyXList.Add(cellX + playerIndexOdd * 1);
                }
            }

            //6到9
            else
            {
                //上边
                topographyXList.Add(cellX);
                topographyYList.Add(cellY + playerIndexOdd * 1);

                //上边的左边
                topographyXList.Add(cellX - playerIndexOdd * 1);
                topographyYList.Add(cellY + playerIndexOdd * 1);

                if (uiName <= 9)
                {
                    topographyYList.Add(cellY - playerIndexOdd * 1);

                    if (uiName == 7)
                        topographyXList.Add(cellX);

                    if (uiName == 8)
                        topographyXList.Add(cellX + playerIndexOdd * 1);

                    if (uiName == 9)
                        topographyXList.Add(cellX + playerIndexOdd * 2);
                }
                else
                {
                    topographyXList.Add(cellX - playerIndexOdd * 2);
                    topographyYList.Add(cellY + playerIndexOdd * 1);
                }
            }
        }

        //最后一个
        else
        {
            topographyXList.Add(cellX);
            topographyYList.Add(cellY + playerIndexOdd * 1);

            topographyXList.Add(cellX - playerIndexOdd * 1);
            topographyYList.Add(cellY + playerIndexOdd * 1);

            topographyXList.Add(cellX + playerIndexOdd * 1);
            topographyYList.Add(cellY - playerIndexOdd * 1);

            topographyXList.Add(cellX + playerIndexOdd * 2);
            topographyYList.Add(cellY - playerIndexOdd * 1);
        }

        Rotate(netCubeInUI.collider.gameObject.transform.localRotation.eulerAngles.z, netCubeInUI.collider.gameObject.transform.localRotation.eulerAngles.y);

        //因为索引0是自己 如果不是空已经放不下骰子了
        for (int i = 5; i >= 1; i--)
        {
            //最下一行不可以生成
            if (topographyXList[i] > 14 || topographyYList[i] > 13 || topographyXList[i] < 0 || topographyYList[i] < 1 || CellParameter.CellInformation[topographyXList[i], topographyYList[i]].Name != ConstantParameter.EMPTYCELL)
            {
                topographyXList.RemoveAt(i);
                topographyYList.RemoveAt(i);
            }
        }
    }

    //玩家旋转之后要变
    private void Rotate(float angleZ, float angleY)
    {
        if (angleY != 0)
        {
            for (int i = 1; i < 6; i++)
            {
                topographyXList[i] = topographyXList[0] + topographyXList[0] - topographyXList[i];
            }
        }

        //旋转后只要调整其他五个就行了 底边自己不变
        if (angleZ != 0f)
        {
            int cosAngleZ = (int)Mathf.Cos(angleZ / 180f * Mathf.PI);

            int sinAngleZ = (int)Mathf.Sin(angleZ / 180f * Mathf.PI);

            int x, y;

            for (int i = 1; i < 6; i++)
            {
                x = topographyXList[i];
                y = topographyYList[i];

                topographyXList[i] = (int)((x - topographyXList[0]) * cosAngleZ - (y - topographyYList[0]) * sinAngleZ + topographyXList[0]);

                topographyYList[i] = (int)((x - topographyXList[0]) * sinAngleZ + (y - topographyYList[0]) * cosAngleZ + topographyYList[0]);
            }

            //if (angleZ == 90f)
            //    for (int i = 1; i < 6; i++)
            //    {
            //        x = topographyX[i];
            //        y = topographyY[i];

            //        topographyX[i] = topographyY[0] - y + topographyX[0];
            //        topographyY[i] = x - topographyX[0] + topographyY[0];
            //    }

            //else if (angleZ == 180f)
            //    for (int i = 1; i < 6; i++)
            //    {
            //        topographyX[i] = 2 * topographyY[0] - topographyX[i];
            //        topographyY[i] = 2 * topographyY[0] - topographyY[i];
            //    }

            //else if (angleZ == 270f)
            //    for (int i = 1; i < 6; i++)
            //    {
            //        x = topographyX[i];
            //        y = topographyY[i];

            //        topographyX[i] = y - topographyY[0] + topographyX[0];
            //        topographyY[i] = topographyX[0] - x + topographyY[0];
            //    }
        }
    }
}
