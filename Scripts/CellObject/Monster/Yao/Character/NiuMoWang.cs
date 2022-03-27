using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiuMoWang : Yao
{
    private const float hp = 2500f;                         //生命值
    private const float attackDamage = 800f;                //攻击力
    private const float armor = 100f;                       //护甲(伤害判定率为n/(1+n))
    private const float magicResistance = 100f;             //魔抗(伤害判定率为n/(1+n))
    private const float shield = 0f;                        //护盾
    private const int mobility = 2;                         //机动性(移动多少格)
    private const int basicAttackDistance = 1;              //攻击距离
    private const int abilityAttackDistance = 4;            //技能攻击距离
    
    //技能倍数
    private const float abilityRate = 1f;

    //妖族天赋普攻伤害1.5倍
    private const float yao_BasicAttackMutiple = 1.5f;

}
