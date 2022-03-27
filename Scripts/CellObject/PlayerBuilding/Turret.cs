using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : CellObject
{
    //�������ķ�������
    private const int turretDefenceSquare = 2;
    private const float constAttackDamage = 50f;

    private int cellX, cellY;   //�Լ�����λ��

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
        Armor = ConstantParameter.damageReductionParameterInRate; //�����ʵ�˺����˺�ֻ��һ��
    }

    public override float[] BasicAttack()
    {
        return new float[2] { 0f, AttackDamage };
    }

    public override void BeBasicAttacked(float physicalAttack, float trueAttack)
    {
        //�е��˻��ױ�Ϊ0
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

        //���׻ָ�
        Armor = ConstantParameter.damageReductionParameterInRate;
    }

    //�����˺���Զֻ��һ��
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

            //����Բ2�ķ�Χ��2��Ȧ������
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

            //��ʼ������ƽ���˺���
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
