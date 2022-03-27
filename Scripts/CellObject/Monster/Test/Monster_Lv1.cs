using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Lv1 : Monster
{
    private const int hpMax = 80;

    public Monster_Lv1()
    {
        this.Level = 1;
        this.HpMax = hpMax;
        this.Hp = hpMax;
        this.AttackDamage = 50f;
        this.Armor = 10f;
        this.BasicAttackDistance = 1;
        this.AbilityAttackDistance = 4;
        this.AbilityRate = 1.8f;
    }
}
