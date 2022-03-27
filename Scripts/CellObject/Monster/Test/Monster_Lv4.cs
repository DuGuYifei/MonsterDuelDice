using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Lv4 : Monster
{
    private const int hpMax = 170;

    public Monster_Lv4()
    {
        this.Level = 4;
        this.HpMax = hpMax;
        this.Hp = hpMax;
        this.AttackDamage = 80f;
        this.Armor = 40f;
        this.BasicAttackDistance = 1;
        this.AbilityAttackDistance = 4;
        this.AbilityRate = 1.8f;
    }
}
