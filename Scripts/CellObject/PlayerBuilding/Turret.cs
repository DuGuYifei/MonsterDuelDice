using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : CellObject
{
    //防御塔的防御距离
    private const int turretDefenceSquare = 2;
    private const float constAttackDamage = 50f;

    private int cellX, cellY;   //自己所在位置

    public int CellX { get => cellX; set => cellX = value; }
    public int CellY { get => cellY; set => cellY = value; }

    public Turret(int x, int y, Slider[] hpSliders)
    {
        cellX = x;
        cellY = y;
        HpSlider = hpSliders;
        HpMax = 300;
        Hp = 300;
        AttackDamage = constAttackDamage;
        Armor = ConstantParameter.damageReductionParameterInRate; //如非真实伤害，伤害只有一半
    }

    public override float[] BasicAttack()
    {
        return new float[2] { 0f, AttackDamage };
    }

    public override void BeBasicAttacked(float physicalAttack, float trueAttack)
    {
        //有敌人护甲变为0
        for(int i = cellX-turretDefenceSquare; i<= cellX+turretDefenceSquare; i++)
        {
            for(int j =cellY + turretDefenceSquare; j<= cellY - turretDefenceSquare; j--)
            {
                if (i == cellX && j == cellY)
                    continue;

                if (CellParameter.CellInformation[i, j].PlayerIndex != -1 && CellParameter.CellInformation[i, j].PlayerIndex != CellParameter.CellInformation[cellX,cellY].PlayerIndex)
                {
                    Armor = 0f;
                    break;
                }
            }
        }

        base.BeBasicAttacked(physicalAttack, trueAttack);

        //护甲恢复
        Armor = ConstantParameter.damageReductionParameterInRate;
    }

    //技能伤害永远只有一半
    public override void BeAbilityAttacked(float physicalAttack, float trueAttack)
    {
        HpChange(-1);
    }   

    public override float[] AttackTalent()
    {
        if (Hp > 0)
        {
            List<CellPosition> target = new List<CellPosition>();

            AttackFunction attackFunction = new AttackFunction();

            //将方圆2的范围（2个圈）加入
            for (int i = cellX - turretDefenceSquare; i <= cellX + turretDefenceSquare; i++)
            {
                for (int j = cellY + turretDefenceSquare; j <= cellY - turretDefenceSquare; j--)
                {
                    if (CellParameter.CellInformation[i, j].PlayerIndex != -1 && CellParameter.CellInformation[i, j].PlayerIndex != CellParameter.CellInformation[cellX, cellY].PlayerIndex)
                    {
                        target.Add(new CellPosition(i, j));
                    }
                }
            }

            //开始攻击（平分伤害）
            AttackDamage /= target.Count;

            foreach(CellPosition element in target)
            {
                attackFunction.NonCounterBasicAttack(cellX, cellY, element.X, element.Z, null);
            }

            AttackDamage = constAttackDamage;
        }

        return new float[0];
    }
}
