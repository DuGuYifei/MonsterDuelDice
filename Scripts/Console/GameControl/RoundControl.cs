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

    //���ڰ���R��һ�غϵ��¼�ͬһ����
    public event Action NextRound;

    void Awake()
    {
        cellFunction = new CellFunction();

        //���һ��Ҫ��һ��init ��Ϊ���õ��������
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

            //����������
            TurretNextRound();

            //�������Ѵ��ڵĹ�����и���״̬�ı�(Ҫ��activeplayerindex�ı�֮ǰ)
            DebuffCheck();

            //��������л�
            ChangeActivePlayer();

            //���������Ƚ��е��������ڼ��������ר�õĽű���
            ChangePlayerCameraDepth();

            //��Ӫ���
            Barracks();
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void TurretNextRound()
    {
        CellParameter.CellInformation[ConstantParameter.NXTowerX, ConstantParameter.NYTowerY].ObjectProperty.AttackTalent();
        CellParameter.CellInformation[ConstantParameter.PXTowerX, ConstantParameter.NYTowerY].ObjectProperty.AttackTalent();
        CellParameter.CellInformation[ConstantParameter.NXTowerX, ConstantParameter.PYTowerY].ObjectProperty.AttackTalent();
        CellParameter.CellInformation[ConstantParameter.PXTowerX, ConstantParameter.PYTowerY].ObjectProperty.AttackTalent();
    }

    /// <summary>
    /// �������Ѵ��ڵĹ�����и���״̬�ı�(Ҫ��activeplayerindex�ı�֮ǰ)
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

                    //        Debug.Log(deBuffDictElement.Key +"ʣ��غ�" + CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff[deBuffDictElement.Key]);
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

                            //Debug.Log(keys[i] + "ʣ��غ�" + CellParameter.CellInformation[cellPositionElement.X, cellPositionElement.Z].ObjectProperty.DeBuff[keys[i]]);
                        }
                    }


                }
            }
        }
    }

    /// <summary>
    /// ��������л�
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
    /// ���������Ƚ��е��������ڼ��������ר�õĽű���
    /// </summary>
    private void ChangePlayerCameraDepth()
    {
        float lastPlayerCameraDepth = Camera.allCameras[0].depth;
        Camera.allCameras[0].depth = Camera.allCameras[1].depth;
        Camera.allCameras[0].depth = lastPlayerCameraDepth;
    }

    /// <summary>
    /// ����ǰ���Ǹ�λ�����û�йֻ�ÿ�غ��Զ�����һ���֡�
    /// �ӵڶ�����������ҷ���ʼ��
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
