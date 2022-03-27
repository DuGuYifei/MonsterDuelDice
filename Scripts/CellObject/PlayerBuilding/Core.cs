using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Core : CellObject
{
    public Core(Slider[] hpSliders)
    {
        HpSlider = hpSliders;
        HpMax = 3;
        Hp = 3;
    }

    public override void BeBasicAttacked(float physicalAttack, float trueAttack)
    {
        HpChange(-1);
        if(DeathDetect())
        {
            SceneManager.LoadScene("StartUI");
        }
    }

    public override void BeAbilityAttacked(float physicalAttack, float trueAttack)
    {
        return;
    }

    public override bool DeathDetect()
    {
        if (this.Hp <= 0)
        {
            //ÓÎÏ·½áÊø

            return true;
        }
        else
            return false;
    }
}
