using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitCheckerboard
{
    private CellFunction cellFunction;

    public void Init(GameObject[] P1_Core_S,
                     GameObject[] P1_NXTower_S,
                     GameObject[] P1_PXTower_S,

                     GameObject[] P2_Core_S,
                     GameObject[] P2_NXTower_S,
                     GameObject[] P2_PXTower_S,

                     GameObject P1_Core_L,
                     GameObject P1_NXTower_L,
                     GameObject P1_PXTower_L,
                     GameObject P2_Core_L,
                     GameObject P2_NXTower_L,
                     GameObject P2_PXTower_L)
    {
        StaticGameObject.battleGround = GameObject.Find("BattleGround");

        StaticGameObject.checkerboard = new List<GameObject>();

        StaticGameObject.checkerboard.Add(GameObject.Find("Checkerboard_P1"));
        StaticGameObject.checkerboard.Add(GameObject.Find("Checkerboard_P2"));

        //Initialize cells in checkerboard
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                CellParameter.CellInformation[i, j] = new CellInfo();
            }
        }

        //给防御塔和能量核心初始化
        CellParameter.CellInformation[ConstantParameter.NXTowerX, ConstantParameter.NYTowerY]
            = new CellInfo("P1_NXTower", 0, P1_NXTower_S, P1_NXTower_L, new Turret(ConstantParameter.NXTowerX, ConstantParameter.NYTowerY, new Slider[2] { P1_NXTower_S[0].GetComponentInChildren<Slider>(), P1_NXTower_S[1].GetComponentInChildren<Slider>() }));

        CellParameter.CellInformation[ConstantParameter.PXTowerX, ConstantParameter.NYTowerY]
            = new CellInfo("P1_PXTower", 0, P1_PXTower_S, P1_PXTower_L, new Turret(ConstantParameter.PXTowerX, ConstantParameter.NYTowerY, new Slider[2] { P1_PXTower_S[0].GetComponentInChildren<Slider>(), P1_PXTower_S[1].GetComponentInChildren<Slider>() }));

        CellParameter.CellInformation[7, 0] = new CellInfo("P1_Core", 0, P1_Core_S, P1_Core_L, new Core(new Slider[2] { P1_Core_S[0].GetComponentInChildren<Slider>(), P1_Core_S[1].GetComponentInChildren<Slider>() }));


        CellParameter.CellInformation[ConstantParameter.NXTowerX, ConstantParameter.PYTowerY]
            = new CellInfo("P2_NXTower", 1, P2_NXTower_S, P2_NXTower_L, new Turret(ConstantParameter.NXTowerX, ConstantParameter.PYTowerY, new Slider[2] { P2_NXTower_S[0].GetComponentInChildren<Slider>(), P2_NXTower_S[1].GetComponentInChildren<Slider>() }));

        CellParameter.CellInformation[ConstantParameter.PXTowerX, ConstantParameter.PYTowerY]
            = new CellInfo("P2_PXTower", 1, P2_PXTower_S, P2_PXTower_L, new Turret(ConstantParameter.PXTowerX, ConstantParameter.PYTowerY, new Slider[2] { P2_PXTower_S[0].GetComponentInChildren<Slider>(), P2_PXTower_S[1].GetComponentInChildren<Slider>() }));

        CellParameter.CellInformation[7, 14] = new CellInfo("P2_Core", 1, P2_Core_S, P2_Core_L, new Core(new Slider[2] { P2_Core_S[0].GetComponentInChildren<Slider>(), P2_Core_S[1].GetComponentInChildren<Slider>() }));


        //预备的三个怪必须在棋盘静态变量之后
        cellFunction = new CellFunction();

        for (int i = 0; i < PlayerParameter.PlayerNum; i++)
        {
            int cellY = ConstantParameter.NYTowerY + i % 2 * (ConstantParameter.PYTowerY - ConstantParameter.NYTowerY);
            cellFunction.NewCellObject(i, 5, cellY, PlayerParameter.Player[i].Monster_S_Name[0]);
            cellFunction.NewCellObject(i, 7, cellY, PlayerParameter.Player[i].Monster_S_Name[0]);
            cellFunction.NewCellObject(i, 9, cellY, PlayerParameter.Player[i].Monster_S_Name[0]);
        }
    }
}
