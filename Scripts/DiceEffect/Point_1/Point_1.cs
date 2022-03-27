using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class Point_1 : MonoBehaviour
{
    //private GameObject smallMonster;
    //private GameObject largeMonster;
    //private GameObject checkerboard;
    //private GameObject battleground;
    
    private string newMonsterName;

    private int cellX, cellY;

    private CellFunction cellFunction = new CellFunction();
    
    void Start()
    {
        newMonsterName = PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[0];
    }

    //void Start()
    //{
    //smallMonster = Resources.Load<GameObject>(smallMonsterPath);
    //largeMonster = Resources.Load<GameObject>(largeMonsterPath);

    //smallMonster = PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[newMonsterName].PrefabOfMonster_S;
    //largeMonster = PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[newMonsterName].PrefabOfMonster_L;

    //largeMonster.transform.localScale *= 30f;

    //checkerboard = StaticGameObject.checkerboard_P1;
    //battleground = StaticGameObject.battleGround;

    //SceneManager.MoveGameObjectToScene(largeMonster, SceneManager.GetSceneByName("BattleGround"));
    //SceneManager.MoveGameObjectToScene(smallMonster, SceneManager.GetSceneByName("BattleGround"));
    //}

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
                cellFunction.NewCellObject(PlayerParameter.ActivePlayerIndex, cellX, cellY, newMonsterName);
            }
            
            Destroy(gameObject);


            ////根据骰字位置得到怪物该召唤在哪儿
            //Vector3 smallMonsterPosition = new Vector3(transform.position.x, 0, transform.position.z);
            //Vector3 largeMonsterPostion = new Vector3(transform.position.x * ConstantParameter.times_GroundToBoard, ConstantParameter.battleGroundY, transform.position.z * ConstantParameter.times_GroundToBoard + ConstantParameter.battleGroundZ);

                ////diceheight 其实和格子边长一样长
                ////if 骰子超出了倒数三行 或者 放置的格子里有东西
                //if (!((smallMonsterPosition.z > ConstantParameter.diceHeight * 3f && smallMonsterPosition.z < ConstantParameter.distance_P1_P2 + ConstantParameter.diceHeight * 12) || CellParameter.CellInformation[(int)smallMonsterPosition.x + 7, (int)smallMonsterPosition.z].Name != ConstantParameter.EMPTYCELL))
                //{
                //    smallMonster.transform.position = smallMonsterPosition;
                //    largeMonster.transform.position = largeMonsterPostion;

                //    //实例化怪物，小的在棋盘上，大的在战场上
                //    smallMonster = Instantiate(smallMonster);
                //    largeMonster = Instantiate(largeMonster);

                //    //将怪物放到父物体上
                //    smallMonster.transform.parent = checkerboard.transform;
                //    largeMonster.transform.parent = battleground.transform;

                //    //在格子静态变量中放上怪物
                //    CellParameter.CellInformation[(int)smallMonster.transform.localPosition.x, (int)smallMonster.transform.localPosition.z] = new CellInfo(newMonsterName, PlayerParameter.ActivePlayerIndex, smallMonster, largeMonster, (Monster)Activator.CreateInstance(Type.GetType(ObjectClassNameParameter.className[newMonsterName])));
                //    //Debug.Log("这个怪物在（" + (int)smallMonster.transform.localPosition.x + "," + (int)smallMonster.transform.localPosition.z + ")");

                //    //在玩家静态变量中使怪物牌堆中的一级怪数量加一
                //    //PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[newMonsterName].Num = PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[newMonsterName].Num + 1;
                //    MonsterData medium = PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[newMonsterName];
                //    medium.Num++;
                //    PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[newMonsterName] = medium;

                //    //在玩家静态变量中使怪物牌堆中的一级怪物位置索引list中加一个新位置
                //    PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[newMonsterName].MonsterCellIndexList.Add(new MonsterCellIndex((int)smallMonster.transform.localPosition.x, (int)smallMonster.transform.localPosition.z));

                //    //Destroy(gameObject);
                //}

                //Destroy(gameObject);

                ////else
                ////{
                ////bool findEmptyCell = false;             //用于退出嵌套循环
                //////找到倒数三行可以放的地方
                ////for (int cellY = 2; cellY >= 0; cellY--)
                ////{ x
                ////    for (int cellX = 0; cellX < 15; cellX++)
                ////    {
                ////        if (CellParameter.Cell[cellX, cellY].Name == ConstantParameter.EMPTYCELL)
                ////        {
                ////            smallMonster.transform.position = new Vector3(cellX - 7, 0, cellY + ConstantParameter.diceHeight / 2);
                ////            largeMonster.transform.position = new Vector3(smallMonster.transform.position.x * ConstantParameter.times_GroundToBoard, ConstantParameter.battleGroundY, smallMonster.transform.position.z * ConstantParameter.times_GroundToBoard + ConstantParameter.battleGroundZ);
                ////            findEmptyCell = true;
                ////            break;
                ////        }
                ////    }

                ////    //找到了就退出
                ////    if (findEmptyCell)
                ////        break;
                ////}

                ////Destroy(smallMonster);
                ////Destroy(largeMonster);

                //////如果后三行没有空位了，就直接结束
                ////if (!findEmptyCell)
                ////Destroy(gameObject);

                ////提示：召唤失败
                ////}


                ////Destroy(gameObject);


                //////实例化怪物，小的在棋盘上，大的在战场上
                ////smallMonster = Instantiate(smallMonster);
                ////largeMonster = Instantiate(largeMonster);

                //////将怪物放到父物体上
                ////smallMonster.transform.parent = checkerboard.transform;
                ////largeMonster.transform.parent = battleground.transform;

                //////在格子静态变量中放上怪物
                ////CellParameter.Cell[(int)smallMonster.transform.localPosition.x, (int)smallMonster.transform.localPosition.z] = new CellObject(PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterPile[0].Name, PlayerParameter.ActivePlayerIndex);

                //////在玩家静态变量中使怪物牌堆中的一级怪数量加一
                ////PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterPile[0].Num++;
                //////在玩家静态变量中使怪物牌堆中的一级怪物位置索引list中加一个新位置
                ////PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterPile[0].MonsterIndexList.Add(new MonsterCellIndex((int)smallMonster.transform.localPosition.x, (int)smallMonster.transform.localPosition.z));
                ////CellParameter.test = smallMonster;
                ////Destroy(CellParameter.test);
                ////Destroy(gameObject);
        }
    }
}
