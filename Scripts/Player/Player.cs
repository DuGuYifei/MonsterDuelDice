using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家携带的怪物信息
/// </summary>
public struct MonsterData
{
    private int _num;
    private GameObject _prefabOfMonster_S;                       //怪物小型prefab的GameObject
    private GameObject _prefabOfMonster_L;                       //怪物大型prefab的GameObject
    private List<CellPosition> _monsterCellIndexList;       //怪物的小棋盘坐标

    public int Num { get => _num; set => _num = value; }
    public GameObject PrefabOfMonster_S { get => _prefabOfMonster_S; set => _prefabOfMonster_S = value; }
    public GameObject PrefabOfMonster_L { get => _prefabOfMonster_L; set => _prefabOfMonster_L = value; }
    public List<CellPosition> MonsterCellIndexList { get => _monsterCellIndexList; set => _monsterCellIndexList = value; }
}

/// <summary>
/// 玩家携带的陷阱信息
/// </summary>
public class TrapsData
{
    private List<string> _trapsName;          //陷阱的名字
    private List<GameObject> _trapPrefab_S;         //小陷阱模型
    private List<GameObject> _trapPrefab_L;         //大陷阱模型

    public List<string> TrapsName { get => _trapsName; set => _trapsName = value; }
    public List<GameObject> TrapPrefab_S { get => _trapPrefab_S; set => _trapPrefab_S = value; }
    public List<GameObject> TrapPrefab_L { get => _trapPrefab_L; set => _trapPrefab_L = value; }
}

/// <summary>
/// 玩家信息
/// </summary>
public class Player
{
    private List<string> monster_S_Name;                //前五个是五个基础等级怪的名字

    private int[] monsterGraveyardNum = new int[5] { 0, 0, 0, 0, 0 };     //索引与monstername对应
    //public string[] monster_L_Name = { "Lv1_L", "Lv2_L", "Lv3_L", "Lv4_L", "Lv5_L" };

    private Dictionary<string, MonsterData> monsterDict;    //string 就是Monster_S_Name

    private string playerAbilityName;                       //玩家携带的技能名

    private TrapsData trapsData;                            //玩家携带的陷阱


    public List<string> Monster_S_Name { get => monster_S_Name; set => monster_S_Name = value; }
    public int[] MonsterGraveyardNum { get => monsterGraveyardNum; set => monsterGraveyardNum = value; }
    public Dictionary<string, MonsterData> MonsterDict { get => monsterDict; set => monsterDict = value; }
    public string PlayerAbilityName { get => playerAbilityName; set => playerAbilityName = value; }
    public TrapsData TrapsData { get => trapsData; set => trapsData = value; }

    public Player(List<string> monstersName, string playerAbility, List<string> trapsName)
    {
        Monster_S_Name = monstersName;
        monsterDict = new Dictionary<string, MonsterData>();
        playerAbilityName = playerAbility;
        trapsData = new TrapsData();

        for (int i = 0; i < Monster_S_Name.Count; i++)
        {
            MonsterData medium = new MonsterData();
            medium.MonsterCellIndexList = new List<CellPosition>();
            medium.PrefabOfMonster_S = Resources.Load<GameObject>(ConstantParameter.MonsterPath + Monster_S_Name[i]);
            medium.PrefabOfMonster_L = Resources.Load<GameObject>(ConstantParameter.MonsterPath + Monster_S_Name[i] + "_L");

            MonsterDict.Add(Monster_S_Name[i], medium);
        }

        trapsData.TrapsName = trapsName;
        trapsData.TrapPrefab_S = new List<GameObject>();
        trapsData.TrapPrefab_L = new List<GameObject>();

        for (int i = 0; i < trapsName.Count; i++)
        {

            trapsData.TrapPrefab_S.Add(Resources.Load<GameObject>(ConstantParameter.TrapPath + trapsName[i]));
            trapsData.TrapPrefab_L.Add(Resources.Load<GameObject>(ConstantParameter.TrapPath + trapsName[i] + "_L"));
        }
    }
}
