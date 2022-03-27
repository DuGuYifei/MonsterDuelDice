using System.Collections.Generic;
using UnityEngine.UI;

public class Monster : CellObject
{
    //激活怪物
    //private bool _activeMonster;                  //是否被激活进行下一步行动

    //怪物等级
    private int _level = 1;

    //怪物属性

    //private int _hp = 0;                            //生命值
    //private int _shield = 0;                        //护盾
    //private float _attackDamage = 0f;               //攻击力
    //private float _armor = 0f;                      //护甲(伤害减免率为n/(1+n))
    ////private float _magicResistance;                 //魔抗(伤害减免率为n/(1+n))
    //private int _basicAttackDistance = 0;           //攻击距离
    private int _abilityAttackDistance = 0;         //技能攻击距离
    private int _abilityEnergy;                 //技能攻击能量格，攒满格释放
    private int energyMax = 2;                  //释放技能所需能量 默认 2

    //private int _mobility = ConstantParameter.moveDistane_Point2_Default;             //2点骰子可移动的点数（改：机动性(移动多少格) 改 是否可以移动（接收2点））
    //private bool _attackability = true;             //是否可以攻击（接收3点）
    //private bool _abilityAttackability = true;      //是否可以放技能（接收4点）

    //技能判定百分比
    private float _abilityRate = 1f;

    //负面效果的持续回合数
    //private int _hellBurden = 0;                    //地狱重负减速效果(修改意见：只有统一的负面效果，和回合数，写一个专门的类，并且减速都是减一样的就行了)
    private Dictionary<string, int> _deBuff =        //存储负面效果的字典
        new Dictionary<string, int>
        {
            { "retard",0}                           //减速
        };

    //private float _physicalAttack = 0;              //物理攻击
    //private float _magicAttack = 0;                 //法术攻击
    //private float _trueAttack = 0;                  //真伤攻击
    //private bool _basicPhysicalAttack = false;      //是否普通物理攻击
    //private bool _basicMagicAttack = false;         //是否普通法术攻击
    //private bool _basicTrueAttack = false;          //是否普通真伤攻击
    //private bool _abilityPhysicalAttack = false;    //是否技能物理攻击
    //private bool _abilityMagicAttack = false;       //是否技能法术攻击
    //private bool _abilityTrueAttack = false;        //是否技能真伤攻击


    //属性接受上面的数值
    //public bool ActiveMonster { get => _activeMonster; set => _activeMonster = value; }
    public int Level { get => _level; set => _level = value; }

    //public int Hp { get => _hp; set => _hp = value; }
    //public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    //public float Armor { get => _armor; set => _armor = value; }
    ////public float MagicResistance { get => _magicResistance; set => _magicResistance = value; }
    //public int Shield { get => _shield; set => _shield = value; }
    //public int BasicAttackDistance { get => _basicAttackDistance; set => _basicAttackDistance = value; }
    public int AbilityAttackDistance { get => _abilityAttackDistance; set => _abilityAttackDistance = value; }
    //public int Mobility { get => _mobility; set => _mobility = value; }
    ////public bool Mobility { get => Mobility1; set => Mobility1 = value; }
    //public bool Attackability { get => _attackability; set => _attackability = value; }
    public int AbilityEnergy { get => _abilityEnergy; set => _abilityEnergy = value; }
    //public bool AbilityAttackability { get => _abilityAttackability; set => _abilityAttackability = value; }
    public int EnergyMax { get => energyMax; set => energyMax = value; }
    public float AbilityRate { get => _abilityRate; set => _abilityRate = value; }


    //public int HellBurden { get => _hellBurden; set => _hellBurden = value; }
    //public Dictionary<string, int> DeBuff { get => _deBuff; set => _deBuff = value; }

    //public float PhysicalAttack { get => _physicalAttack; set => _physicalAttack = value; }
    //public float MagicAttack { get => _magicAttack; set => _magicAttack = value; }
    //public float TrueAttack { get => _trueAttack; set => _trueAttack = value; }
    //public bool BasicPhysicalAttack { get => _basicPhysicalAttack; set => _basicPhysicalAttack = value; }
    //public bool BasicMagicAttack { get => _basicMagicAttack; set => _basicMagicAttack = value; }
    //public bool BasicTrueAttack { get => _basicTrueAttack; set => _basicTrueAttack = value; }
    //public bool AbilityPhysicalAttack { get => _abilityPhysicalAttack; set => _abilityPhysicalAttack = value; }
    //public bool AbilityMagicAttack { get => _abilityMagicAttack; set => _abilityMagicAttack = value; }
    //public bool AbilityTrueAttack { get => _abilityTrueAttack; set => _abilityTrueAttack = value; }

    //构造函数
    public Monster()
    {
        Mobility = ConstantParameter.moveDistane_Point2_Default;
        Attackability = true;
        AbilityAttackability = true;
        AbilityEnergy = EnergyMax;
    }

    //public Monster(int hp, int sheild, float attackDamage, float armor, int basicAttackDistance, int abilityAttackDistance)
    //{
    //    _hp = hp;
    //    _attackDamage = attackDamage;
    //    _armor = armor;
    //    _basicAttackDistance = basicAttackDistance;
    //    _abilityAttackDistance = abilityAttackDistance;
    //}

    //public Monster(int hp, int sheild, float attackDamage, float armor, int basicAttackDistance, int abilityAttackDistance, bool mobility)
    //{
    //    _hp = hp;
    //    _attackDamage = attackDamage;
    //    _armor = armor;
    //    _basicAttackDistance = basicAttackDistance;
    //    _abilityAttackDistance = abilityAttackDistance;
    //    _mobility = mobility;
    //}

    //public Monster(int hp, int sheild, float attackDamage, float armor, int basicAttackDistance, int abilityAttackDistance, bool mobility, float abilityRate)
    //{
    //    _hp = hp;
    //    _attackDamage = attackDamage;
    //    _armor = armor;
    //    _basicAttackDistance = basicAttackDistance;
    //    _abilityAttackDistance = abilityAttackDistance;
    //    _mobility = mobility;
    //    _abilityRate = abilityRate;
    //}


    //获得大类型的名字，方便分辨出这是什么怪


    ////攻击
    ////普通攻击
    //public float[] BasicAttack()
    //{
    //    //用于给敌人接收的伤害值,0是物理，1是真实
    //    float[] attackTrueDamage = new float[2];
    //    attackTrueDamage[0] = AttackDamage;
    //    //attackTrueDamage = AttackTalent();
    //    return attackTrueDamage;
    //}

    public float[] HalfAttack()
    {
        float[] attackTrueDamage = new float[2];
        attackTrueDamage[0] = AttackDamage / 2;

        return attackTrueDamage;
    }

    //技能攻击
    public virtual float[] AbilityAttack()
    {
        AbilityEnergy = 0;
        //用于给敌人接收的伤害值,0是物理，1是真实
        float[] attackTrueDamage = new float[2];
        attackTrueDamage[0] = AbilityRate * AttackDamage;
        return attackTrueDamage;
    }

    ////被攻击转换接收到的伤害
    //public void BeBasicAttacked(float physicalAttack, float trueAttack)//float magicAttack, 
    //{
    //    int damageValue = UnityEngine.Mathf.RoundToInt(physicalAttack * ConstantParameter.damageReductionParameterInRate / (ConstantParameter.damageReductionParameterInRate + Armor) + trueAttack);
    //    //+ magicAttack * MagicResistance / (100 + MagicResistance) 

    //    //如果有受击触发型天赋,已抛弃，因为需要靠controller来控制，万一是免疫物理攻击，有些是伤害值总体免疫（可以直接在两个数值同时乘完免疫率再放进来）
    //    //damageValue = DefenceTalent(damageValue);

    //    //生命值减少计算（护甲值 魔抗值 护盾值）
    //    if (Shield >= damageValue)
    //    {
    //        Shield -= damageValue;
    //    }
    //    else
    //    {
    //        Hp -= (damageValue - Shield);
    //        //+ magicAttack * MagicResistance / (100 + MagicResistance) 

    //        Shield = 0;
    //    }
    //}

    ////被技能攻击时收到的伤害
    //public void BeAbilityAttacked(float physicalAttack, float trueAttack)//float magicAttack, 
    //{
    //    int damageValue = UnityEngine.Mathf.RoundToInt(physicalAttack * ConstantParameter.damageReductionParameterInRate / (ConstantParameter.damageReductionParameterInRate + Armor) + trueAttack);
    //    //+ magicAttack * MagicResistance / (100 + MagicResistance) 

    //    //如果有受击触发型天赋,已抛弃，因为需要靠controller来控制，万一是免疫物理攻击，有些是伤害值总体免疫（可以直接在两个数值同时乘完免疫率再放进来）
    //    //damageValue = DefenceTalent(damageValue);

    //    //生命值减少计算（护甲值 魔抗值 护盾值）
    //    if (Shield >= damageValue)
    //    {
    //        Shield -= damageValue;
    //    }
    //    else
    //    {
    //        Hp -= (damageValue - Shield);
    //        //+ magicAttack * MagicResistance / (100 + MagicResistance) 

    //        Shield = 0;
    //    }
    //}

    ////用虚方法写的攻击触发型天赋和受击触发型天赋,其他非战斗天赋
    //public virtual float[] AttackTalent()
    //{
    //    //用于给敌人接收的伤害值,0是物理，1是真实
    //    float[] attackTrueDamage = new float[2];
    //    return attackTrueDamage;
    //}
    //public virtual float[] DefenceTalent()
    //{
    //    return new float[2];
    //}
    //public virtual void UnCombatTalent()
    //{

    //}

    ////死亡测试
    //public virtual bool DeathDetect()
    //{
    //    if (this.Hp <= 0)
    //        return true;

    //    return false;
    //}

    //攻击
    //public void Attack(Monster victim)
    //{
    //    //计算给予对手的伤害
    //    CalculateAttackValue();
    //    //如果有攻击触发型天赋则重新计算
    //    AttackTalent();

    //    //调用被攻击者的受击函数
    //    victim.BeAttacked(PhysicalAttack, MagicAttack, TrueAttack);

    //    //攻击完让所有攻击数值重新归0，攻击指令回归false，因为不同种族天赋在攻击时会产生不同攻击效果
    //    PhysicalAttack = 0;
    //    MagicAttack = 0;
    //    TrueAttack = 0;
    //    BasicPhysicalAttack = false;
    //    BasicMagicAttack = false;
    //    BasicTrueAttack = false;
    //    AbilityPhysicalAttack = false;
    //    AbilityMagicAttack = false;
    //    AbilityTrueAttack = false;
    //}
}
