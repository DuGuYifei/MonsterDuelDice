using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayer
{
    public void Init()
    {
        ////字典测试
        //Dictionary<int, int> a = new Dictionary<int, int>();
        //a[1]=1;
        //Debug.Log(a[1]);

        PlayerParameter.Player = new Player[2];

        List<string> monstersName = new List<string> { "Lv1", "Lv2", "Lv3", "Lv4", "Lv5" };

        PlayerParameter.PlayerNum = 2;
        List<string> TrapsName = new List<string> { "FireStorm","HellBurden"};   //每个玩家带两个陷阱 5和55（索引号为0和1）

        CellParameter.TrapsPositionList = new List<CellPosition>[PlayerParameter.PlayerNum];
        for(int i = 0;i< PlayerParameter.PlayerNum;i++)
        {
            CellParameter.TrapsPositionList[i] = new List<CellPosition>();
        }

        PlayerParameter.Player[0] = new Player(monstersName, "Annihilate", TrapsName);
        PlayerParameter.Player[1] = new Player(monstersName, "SpringIsComing", TrapsName);
        
        PlayerParameter.ActivePlayerIndex = 0;
    }
}
