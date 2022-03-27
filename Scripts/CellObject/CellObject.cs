using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CellObject
{
    //怪物属性
    private int _hpMax = 1;
    private Slider[] hpSlider;
    private int _hp = 0;                            //生命值
    private int _shield = 0;                        //护盾
    private float _attackDamage = 0f;               //攻击力
    private float _armor = 0f;                      //护甲(伤害减免率为n/(1+n))

    private int _basicAttackDistance = 0;           //攻击距离

    private int _mobility = 0;                      //2点骰子可移动的点数（改：机动性(移动多少格) 改 是否可以移动（接收2点））
    private bool _attackability = false;             //是否可以攻击（接收3点）
    private bool _abilityAttackability = false;      //是否可以放技能（接收4点）


    //负面效果的持续回合数
    private Dictionary<string, int> _deBuff =        //存储负面效果的字典
        new Dictionary<string, int>
        {
            { "retard",0},                           //减速
            { "shackled",0}
        };

    public Slider[] HpSlider { get => hpSlider; set => hpSlider = value; }
    public int HpMax { get => _hpMax; set => _hpMax = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public float Armor { get => _armor; set => _armor = value; }
    public int Shield { get => _shield; set => _shield = value; }
    public int BasicAttackDistance { get => _basicAttackDistance; set => _basicAttackDistance = value; }
    public int Mobility { get => _mobility; set => _mobility = value; }
    public bool Attackability { get => _attackability; set => _attackability = value; }
    //public int AbilityEnergy { get => _abilityEnergy; set => _abilityEnergy = value; }
    public bool AbilityAttackability { get => _abilityAttackability; set => _abilityAttackability = value; }
    public Dictionary<string, int> DeBuff { get => _deBuff; set => _deBuff = value; }

    //普通攻击
    public virtual float[] BasicAttack()
    {
        //用于给敌人接收的伤害值,0是物理，1是真实
        float[] attackTrueDamage = new float[2];
        attackTrueDamage[0] = AttackDamage;
        return attackTrueDamage;
    }

    //被攻击转换接收到的伤害
    public virtual void BeBasicAttacked(float physicalAttack, float trueAttack)//float magicAttack, 
    {
        int damageValue = UnityEngine.Mathf.RoundToInt(physicalAttack * ConstantParameter.damageReductionParameterInRate / (ConstantParameter.damageReductionParameterInRate + Armor) + trueAttack);
       

        //生命值减少计算（护甲值 魔抗值 护盾值）
        if (Shield >= damageValue)
        {
            Shield -= damageValue;
        }
        else
        {
            HpChange(-(damageValue - Shield));

            Shield = 0;
        }
    }

    //被技能攻击时收到的伤害
    public virtual void BeAbilityAttacked(float physicalAttack, float trueAttack)//float magicAttack, 
    {
        int damageValue = UnityEngine.Mathf.RoundToInt(physicalAttack * ConstantParameter.damageReductionParameterInRate / (ConstantParameter.damageReductionParameterInRate + Armor) + trueAttack);


        //生命值减少计算（护甲值 魔抗值 护盾值）
        if (Shield >= damageValue)
        {
            Shield -= damageValue;
        }
        else
        {
            HpChange(-(damageValue - Shield));

            Shield = 0;
        }
    }

    //用虚方法写的攻击触发型天赋和受击触发型天赋,其他非战斗天赋
    public virtual float[] AttackTalent()
    {
        //用于给敌人接收的伤害值,0是物理，1是真实
        float[] attackTrueDamage = new float[2];
        return attackTrueDamage;
    }
    public virtual float[] DefenceTalent()
    {
        return new float[2];
    }
    public virtual void UnCombatTalent()
    {

    }

    //死亡测试
    public virtual bool DeathDetect()
    {
        if (this.Hp <= 0)
            return true;

        return false;
    }

    public virtual void HpChange(int num)
    {
        Hp += num;

        HpSlider[0].value = Hp / HpMax;
        HpSlider[1].value = Hp / HpMax;
    }
}
