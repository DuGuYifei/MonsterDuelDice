using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstantParameter
{
    public const string EMPTYCELL = "Empty";
    public const string TOPOGRAPHYOBSTACLE = "Relic";

    //测试用的怪物模型的路径
    //public const string Lv1_SmallMonsterPath = "Prefab/Monster/Test/Lv1";         
    //public const string Lv1_LargeMonsterPath = "Prefab/Monster/Test/Lv1_L";
    //public const string Lv2_SmallMonsterPath = "Prefab/Monster/Test/Lv2";
    //public const string Lv2_LargeMonsterPath = "Prefab/Monster/Test/Lv2_L";
    //public const string Lv3_SmallMonsterPath = "Prefab/Monster/Test/Lv3";
    //public const string Lv3_LargeMonsterPath = "Prefab/Monster/Test/Lv3_L";
    //public const string Lv4_SmallMonsterPath = "Prefab/Monster/Test/Lv4";
    //public const string Lv4_LargeMonsterPath = "Prefab/Monster/Test/Lv4_L";
    //public const string Lv5_SmallMonsterPath = "Prefab/Monster/Test/Lv5";
    //public const string Lv5_LargeMonsterPath = "Prefab/Monster/Test/Lv5_L";

    public const string ModelPath = "Prefab/";
    public const string MonsterPath = ModelPath + "Monster/Test/";
    public const string TrapPath = ModelPath + "Trap/";
    public const string TopographyObstaclePath = ModelPath + "TopographyMagic/Relic";

    //摄像机参数
    public static readonly Vector3[] PlayerCameraPosition = new Vector3[2] { new Vector3(0f,12f,-1.8f), new Vector3(0f,12f,511.8f) };

    //棋盘参数
    public const float diceHeight = 1f;
    public const float battleGroundY = -30f;
    public const float battleGroundZ = 30f;
    public const float times_GroundToBoard = 30f;               //战斗场地对棋盘的倍数
    public const float distance_P1_P2 = 495f;

    //防御塔位置
    public const int NXTowerX = 4;
    public const int NYTowerY = 2;
    public const int PXTowerX = 10;
    public const int PYTowerY = 12;

    //怪物参数
    public const float damageReductionParameterInRate = 100f;   //伤害抵消率

    //怪物移动
    public const float moveTime = 10f;                          //怪物移动一格所需要的帧数
    public const int moveDistane_Point2_Default = 3;            //点数2移动的距离
    public const int moveDistane_Point222 = 8;                  //点数2移动的距离

    //怪物技能能量值（取消，因为部分怪物可以技能更强大导致不止两格）
    //public const int EnergyMax = 2;
}

public class StaticGameObject
{
    public static GameObject UIDiceParentObject;

    public static List<GameObject> checkerboard;

    //public static GameObject checkerboard_P2;

    public static GameObject battleGround;
}

public class ThrowDiceParameter
{
    private static bool[] newDice = { false, false, false };    //是否三个骰子都扔结束了，且都是未用过的
    private static int[] diceThrowResult = { 0, 0, 0 };         //扔出来的结果

    public static bool[] NewDice { get => newDice; set => newDice = value; }
    public static int[] DiceThrowResult { get => diceThrowResult; set => diceThrowResult = value; }
}

public class SummonDiceParameter
{
    private static IDictionary<string, bool> located = new Dictionary<string, bool>();    //召唤出来的骰子找到地点了，静态变量located是用来判定是否放弃先使用该骰子

    public static IDictionary<string, bool> Located { get => located; set => located = value; }
}

public class CellParameter
{
    private static CellInfo[,] cellInformation = new CellInfo[15, 15];              //15行，15列

    //陷阱
    private static dynamic[,] trapsProperty = new dynamic[15, 15];     //15行，15列
    private static List<CellPosition>[] trapsPositionList;             //所有玩家的陷阱的位置list，初始化在InitPlayer里，因为那里决定了多少玩家


    public static CellInfo[,] CellInformation { get => cellInformation; set => cellInformation = value; }
    public static dynamic[,] TrapsProperty { get => trapsProperty; set => trapsProperty = value; }
    public static List<CellPosition>[] TrapsPositionList { get => trapsPositionList; set => trapsPositionList = value; }
}

public class PlayerParameter
{
    private static int playerNum;
    private static Player[] player;
    private static int activePlayerIndex;  //偶数P1，奇数P2(因为有可能不止两个玩家以后)                         

    public static Player[] Player { get => player; set => player = value; }
    public static int ActivePlayerIndex { get => activePlayerIndex; set => activePlayerIndex = value; }
    public static int PlayerNum { get => playerNum; set => playerNum = value; }
}

public class ObjectClassNameParameter
{
    public static Dictionary<string, string> className = new Dictionary<string, string>
    {
        {"Lv1", "Monster_Lv1"},
        {"Lv2", "Monster_Lv2"},
        {"Lv3", "Monster_Lv3"},
        {"Lv4", "Monster_Lv4"},
        {"Lv5", "Monster_Lv5"},
        {"FireStorm", "Trap_FireStorm"},
        {"HellBurden", "Trap_HellBurden"}
    };
}

public class StartMenuParameter
{
    private static bool flag_escapeToStartMenu = false;

    public static bool Flag_escapeToStartMenu { get => flag_escapeToStartMenu; set => flag_escapeToStartMenu = value; }
}