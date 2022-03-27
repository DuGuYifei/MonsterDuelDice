using UnityEngine;

public class CellPosition
{
    private int x;
    private int z;

    //用于寻路算法
    private int gCost, hCost, fCost;
    private CellPosition parent;

    public int X { get => x; set => x = value; }
    public int Z { get => z; set => z = value; }
    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public int FCost { get => fCost; set => fCost = value; }
    public CellPosition Parent { get => parent; set => parent = value; }

    public CellPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.gCost = 0;
        this.hCost = 0;
        this.fCost = 0;
        this.parent = null;
    }

    //public CellPosition(CellPosition cp)
    //{
    //    this.x = cp.x;
    //    this.z = cp.z;
    //    this.gCost = cp.gCost;
    //    this.hCost = cp.hCost;
    //    this.fCost = cp.fCost;
    //    this.parent = cp.parent;
    //}

    public void CalculateCosts(CellPosition start, CellPosition end)
    {
        this.gCost = Mathf.Abs(start.X - X) + Mathf.Abs(start.Z - Z);
        this.hCost = Mathf.Abs(end.X - X) + Mathf.Abs(end.Z - Z);
        this.fCost = gCost + hCost;
    }
}


public class CellInfo
{
    private string _name;               //用于知道该格子物体是什么，以及是否为空
    private int _playerIndex;           //该格子物体属于哪个玩家(-1空)
    private GameObject[] _objectInCell_S;
    private GameObject _objectInCell_L;
    private dynamic _objectProperty;

    public string Name { get => _name; set => _name = value; }
    public int PlayerIndex { get => _playerIndex; set => _playerIndex = value; }
    public GameObject[] ObjectInCell_S { get => _objectInCell_S; set => _objectInCell_S = value; }
    public GameObject ObjectInCell_L { get => _objectInCell_L; set => _objectInCell_L = value; }
    public dynamic ObjectProperty { get => _objectProperty; set => _objectProperty = value; }
    

    public CellInfo()
    {
        this.Name = ConstantParameter.EMPTYCELL;    //空的格子
        this.PlayerIndex = -1;                      //空的格子不属于任何人
        this.ObjectProperty = null;
    }

    public CellInfo(string name, int playerIndex, GameObject[] gameObject_S_P1, GameObject gameObject_L, dynamic objectProperty)
    {
        this.Name = name;
        this.PlayerIndex = playerIndex;
        this.ObjectInCell_S = gameObject_S_P1;
        this.ObjectInCell_L = gameObject_L;
        this.ObjectProperty = objectProperty;
    }

    //public CellInfo(string name, int playerIndex, GameObject[] gameObject_S, GameObject gameObject_L)
    //{
    //    this.Name = name;
    //    this.PlayerIndex = playerIndex;
    //    this.ObjectInCell_S = gameObject_S;
    //    this.ObjectInCell_L = gameObject_L;
    //}
}
