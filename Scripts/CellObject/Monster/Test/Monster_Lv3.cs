using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Lv3:Monster
{
    private const int hpMax = 130;

    public Monster_Lv3()
    {
        this.Level = 3;
        this.HpMax = hpMax;
        this.Hp = hpMax;
        this.AttackDamage = 70f;
        this.Armor = 30f;
        this.BasicAttackDistance = 1;
        this.AbilityAttackDistance = 4;
        this.AbilityRate = 1.8f;
    }
}
