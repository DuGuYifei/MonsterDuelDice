using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap
{
    private int _playerIndex;
    private GameObject[] trap_S;     //����С����ģ�� �����ϵ�
    private GameObject trap_L;       //ս���ϵĴ�ģ��

    public int PlayerIndex { get => _playerIndex; set => _playerIndex = value; }
    public GameObject[] Trap_S { get => trap_S; set => trap_S = value; }
    public GameObject Trap_L { get => trap_L; set => trap_L = value; }
}
