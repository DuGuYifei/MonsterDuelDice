using System;
using UnityEngine;
/// <summary>
/// 火焰风暴陷阱 造成100真实伤害等同于Lv2的最大血量
/// 一定要有playerIndex
/// </summary>
public class Trap_FireStorm : Trap
{
    private CellFunction cellFunction = new CellFunction();
    private TrapCellFunction trapCellFunction = new TrapCellFunction();
    
    private int _trueDamage = 80;   //设置为Lv1的最大学血量

    public int TrueDamage { get => _trueDamage; set => _trueDamage = value; }

    public void Explode(int targetX, int targetY)
    {
        CellParameter.CellInformation[targetX, targetY].ObjectProperty.Hp -= TrueDamage;

        //Debug.Log("陷阱爆炸"+CellParameter.CellInformation[targetX, targetY].ObjectProperty.Hp);

        if (CellParameter.CellInformation[targetX, targetY].ObjectProperty.DeathDetect())
        {
            cellFunction.DestroyCellObject(targetX, targetY);
        }

        trapCellFunction.DestroyTrapObject(targetX, targetY);
    }
}
