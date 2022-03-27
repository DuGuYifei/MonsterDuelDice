using System;
using UnityEngine;
/// <summary>
/// ����籩���� ���100��ʵ�˺���ͬ��Lv2�����Ѫ��
/// һ��Ҫ��playerIndex
/// </summary>
public class Trap_FireStorm : Trap
{
    private CellFunction cellFunction = new CellFunction();
    private TrapCellFunction trapCellFunction = new TrapCellFunction();
    
    private int _trueDamage = 80;   //����ΪLv1�����ѧѪ��

    public int TrueDamage { get => _trueDamage; set => _trueDamage = value; }

    public void Explode(int targetX, int targetY)
    {
        CellParameter.CellInformation[targetX, targetY].ObjectProperty.Hp -= TrueDamage;

        //Debug.Log("���屬ը"+CellParameter.CellInformation[targetX, targetY].ObjectProperty.Hp);

        if (CellParameter.CellInformation[targetX, targetY].ObjectProperty.DeathDetect())
        {
            cellFunction.DestroyCellObject(targetX, targetY);
        }

        trapCellFunction.DestroyTrapObject(targetX, targetY);
    }
}
