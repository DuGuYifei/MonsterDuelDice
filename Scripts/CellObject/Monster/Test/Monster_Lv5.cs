using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Lv5 : Monster
{
    private const int hpMax = 250;

    public Monster_Lv5()
    {
        this.Level = 5;
        this.HpMax = hpMax;
        this.Hp = hpMax;
        this.AttackDamage = 120f;
        this.Armor = 60f;
        this.BasicAttackDistance = 1;
        this.AbilityAttackDistance = 4;
        this.AbilityRate = 1.8f;
    }
}
