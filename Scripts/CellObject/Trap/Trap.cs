using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap
{
    private int _playerIndex;
    private GameObject[] trap_S;     //两个小陷阱模型 棋盘上的
    private GameObject trap_L;       //战场上的大模型

    public int PlayerIndex { get => _playerIndex; set => _playerIndex = value; }
    public GameObject[] Trap_S { get => trap_S; set => trap_S = value; }
    public GameObject Trap_L { get => trap_L; set => trap_L = value; }
}
