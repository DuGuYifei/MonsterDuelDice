using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yao : Monster
{
    //妖族天赋普攻造成伤害加倍
    private float _yao_BasicAttackMutiple = 1.5f;

    public float Yao_BasicAttackMutiple { get => _yao_BasicAttackMutiple; set => _yao_BasicAttackMutiple = value; }

    //天赋：造成普攻伤害加倍
    //public override void AttackTalent()
    //{
        //if(BasicPhysicalAttack)
        //{
        //    PhysicalAttack *= Yao_BasicAttackMutiple;
        //}
        //if(BasicMagicAttack)
        //{
        //    MagicAttack *= Yao_BasicAttackMutiple;
        //}
        //if(BasicTrueAttack)
        //{
        //    TrueAttack *= Yao_BasicAttackMutiple;
        //}
    //}
}
