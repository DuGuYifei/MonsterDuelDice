using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Lv2 : Monster
{
    private const int hpMax = 100;

    public Monster_Lv2()
    {
        this.Level = 2;
        this.HpMax = hpMax;
        this.Hp = hpMax;
        this.AttackDamage = 60f;
        this.Armor = 20f;
        this.BasicAttackDistance = 1;
        this.AbilityAttackDistance = 4;
        this.AbilityRate = 1.8f;
    }
}
