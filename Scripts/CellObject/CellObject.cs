using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CellObject
{
    //��������
    private int _hpMax = 1;
    private Slider[] hpSlider;
    private int _hp = 0;                            //����ֵ
    private int _shield = 0;                        //����
    private float _attackDamage = 0f;               //������
    private float _armor = 0f;                      //����(�˺�������Ϊn/(1+n))

    private int _basicAttackDistance = 0;           //��������

    private int _mobility = 0;                      //2�����ӿ��ƶ��ĵ������ģ�������(�ƶ����ٸ�) �� �Ƿ�����ƶ�������2�㣩��
    private bool _attackability = false;             //�Ƿ���Թ���������3�㣩
    private bool _abilityAttackability = false;      //�Ƿ���Էż��ܣ�����4�㣩


    //����Ч���ĳ����غ���
    private Dictionary<string, int> _deBuff =        //�洢����Ч�����ֵ�
        new Dictionary<string, int>
        {
            { "retard",0},                           //����
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

    //��ͨ����
    public virtual float[] BasicAttack()
    {
        //���ڸ����˽��յ��˺�ֵ,0������1����ʵ
        float[] attackTrueDamage = new float[2];
        attackTrueDamage[0] = AttackDamage;
        return attackTrueDamage;
    }

    //������ת�����յ����˺�
    public virtual void BeBasicAttacked(float physicalAttack, float trueAttack)//float magicAttack, 
    {
        int damageValue = UnityEngine.Mathf.RoundToInt(physicalAttack * ConstantParameter.damageReductionParameterInRate / (ConstantParameter.damageReductionParameterInRate + Armor) + trueAttack);
       

        //����ֵ���ټ��㣨����ֵ ħ��ֵ ����ֵ��
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

    //�����ܹ���ʱ�յ����˺�
    public virtual void BeAbilityAttacked(float physicalAttack, float trueAttack)//float magicAttack, 
    {
        int damageValue = UnityEngine.Mathf.RoundToInt(physicalAttack * ConstantParameter.damageReductionParameterInRate / (ConstantParameter.damageReductionParameterInRate + Armor) + trueAttack);


        //����ֵ���ټ��㣨����ֵ ħ��ֵ ����ֵ��
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

    //���鷽��д�Ĺ����������츳���ܻ��������츳,������ս���츳
    public virtual float[] AttackTalent()
    {
        //���ڸ����˽��յ��˺�ֵ,0������1����ʵ
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

    //��������
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
