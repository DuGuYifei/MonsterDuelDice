using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundControl : MonoBehaviour
{
    public GameObject[] P1_Core_S;
    public GameObject[] P1_NXTower_S;
    public GameObject[] P1_PXTower_S;

    public GameObject[] P2_Core_S;
    public GameObject[] P2_NXTower_S;
    public GameObject[] P2_PXTower_S;

    public GameObject P1_Core_L;
    public GameObject P1_NXTower_L;
    public GameObject P1_PXTower_L;
    public GameObject P2_Core_L;
    public GameObject P2_NXTower_L;
    public GameObject P2_PXTower_L;

    private CellFunction cellFunction;

    //用于按下R下一回合的事件同一控制
    public event Action NextRound;

    void Awake()
    {
        cellFunction = new CellFunction();

        //玩家一定要第一个init 因为会用到玩家人数
        InitPlayer initPlayer = new InitPlayer();
        InitCheckerboard initCheckerboard = new InitCheckerboard();
        InitUI initUI = new InitUI();

        initPlayer.Init();
        initCheckerboard.Init(P1_Core_S, P1_NXTower_S, P1_PXTower_S, P2_Core_S, P2_NXTower_S, P2_PXTower_S, P1_Core_L, P1_NXTower_L, P1_PXTower_L, P2_Core_L, P2_NXTower_L, P2_PXTower_L);
        initUI.init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (NextRound != null)
            {
                NextRound();
            }

            //防御塔攻击
            TurretNextRound();

            //对所有已存在的怪物进行负面状态改变(要在activeplayerindex改变之前)
            DebuffCheck();

            //激活玩家切换
            ChangeActivePlayer();

            //摄像机的深度进行调换（后期加入摄像机专用的脚本）
            ChangePlayerCameraDepth();

            //兵营造兵
            Barracks();
        }
    }

    /// <summary>
    /// 防御塔攻击
    /// </summary>
    private void TurretNextRound()
    {
        CellParameter.CellInformation[ConstantParameter.NXTowerX, ConstantParameter.NYTowerY].ObjectProperty.AttackTalent();
        CellParameter.CellInformation[ConstantParameter.PXTowerX, ConstantParameter.NYTowerY].ObjectProperty.AttackTalent();
        CellParameter.CellInformation[ConstantParameter.NXTowerX, ConstantParameter.PYTowerY].ObjectProperty.AttackTalent();
        CellParameter.CellInformation[ConstantParameter.PXTowerX, ConstantParameter.PYTowerY].ObjectProperty.AttackTalent();
    }

    /// <summary>
    /// 对所有已存在的怪物进行负面状态改变(要在activeplayerindex改变之前)
    /// </summary>
    private void DebuffCheck()
    {
        foreach (var monsterDictElement in PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict)
        {
            if (monsterDictElement.Value.Num > 0)
            {
                foreach (var cellPositionElement in monsterDictElement.Value.MonsterCellIndexList)
                {
                    //foreach (KeyValuePair<string, int> deBuffDictElement in CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff)
                    //{
                    //    if (deBuffDictElement.Value > 0)
                    //    {
                    //        CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff[deBuffDictElement.Key]--;

                    //        Debug.Log(deBuffDictElement.Key +"剩余回合" + CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff[deBuffDictElement.Key]);
                    //    }
                    //}

                    List<string> keys = new List<string>();
                    foreach(string element in CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff.Keys)
                    {
                        keys.Add(element);
                    }

                    for(int i =0;i< CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff.Count;i++)
                    {
                        if(CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff[keys[i]]>0)
                        {
                            CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff[keys[i]]--;

                            //Debug.Log(keys[i] + "剩余回合" + CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff[keys[i]]);
                        }
                    }


                }
            }
        }
    }

    /// <summary>
    /// 激活玩家切换
    /// </summary>
    private void ChangeActivePlayer()
    {
        PlayerParameter.ActivePlayerIndex += 1;

        if (PlayerParameter.ActivePlayerIndex == PlayerParameter.PlayerNum)
        {
            PlayerParameter.ActivePlayerIndex = 0;
        }
    }

    /// <summary>
    /// 摄像机的深度进行调换（后期加入摄像机专用的脚本）
    /// </summary>
    private void ChangePlayerCameraDepth()
    {
        float lastPlayerCameraDepth = Camera.allCameras[0].depth;
        Camera.allCameras[0].depth = Camera.allCameras[1].depth;
        Camera.allCameras[0].depth = lastPlayerCameraDepth;
    }

    /// <summary>
    /// 核心前的那个位置如果没有怪会每回合自动生成一个怪。
    /// 从第二个操作的玩家方开始。
    /// </summary>
    private void Barracks()
    {
        int cellY = PlayerParameter.ActivePlayerIndex % 2 * 12 + 1;
        if (CellParameter.CellInformation[7, cellY].Name == ConstantParameter.EMPTYCELL)
        {
            cellFunction.NewCellObject(PlayerParameter.ActivePlayerIndex, 7, cellY, PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name[0]);
        }
    }
}
