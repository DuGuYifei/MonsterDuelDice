using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFunction
{
    private CellFunction cellFunction;

    //好似喘痕方
    //儖孀黍繁
    public bool FindEnemy(List<CellPosition> scope, int currentPlayerIndex)
    {
        foreach (CellPosition cell in scope)
        {
            if (CellParameter.CellInformation[cell.X, cell.Z].PlayerIndex != -1 && CellParameter.CellInformation[cell.X, cell.Z].PlayerIndex != currentPlayerIndex)
                return true;
        }

        return false;
    }


    //泣方4�此�！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    //泣方4才44宥喘�瑳篠楾セ�
    public void AbilityAttack(int originX, int originZ, int targetX, int targetZ, GameObject gameObject)
    {
        //喘噐公黍繁俊辺議彬墾峙,0頁麗尖��1頁寔糞
        float[] attackTrueDamage = new float[2];

        attackTrueDamage = CellParameter.CellInformation[originX, originZ].ObjectProperty.AbilityAttack();

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        CellParameter.CellInformation[targetX, targetZ].ObjectProperty.BeAbilityAttacked(attackTrueDamage[0], attackTrueDamage[1]);

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        DeathDetect(targetX, targetZ);

        StaticGameObject.UIDiceParentObject.SetActive(true);
        UnityEngine.Object.Destroy(gameObject);
    }
    //泣方4�此�！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！



    //泣方3�此�！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    //銭偬曾肝噸好
    public void TwiceBasicAttack(int originX, int originZ, int targetX, int targetZ, GameObject gameObject)
    {
        float[] attackTrueDamage = new float[2];

        attackTrueDamage = CellParameter.CellInformation[originX, originZ].ObjectProperty.HalfAttack();

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        CellParameter.CellInformation[targetX, targetZ].ObjectProperty.BeBasicAttacked(attackTrueDamage[0], attackTrueDamage[1]);

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        DeathDetect(targetX, targetZ);

        BasicAttack(originX, originZ, targetX, targetZ, gameObject);
    }

    //噸好
    public void BasicAttack(int originX, int originZ, int targetX, int targetZ, GameObject gameObject)
    {
        //喘噐公黍繁俊辺議彬墾峙,0頁麗尖��1頁寔糞
        float[] attackTrueDamage = new float[2];

        NonCounterBasicAttack(originX, originZ, targetX, targetZ, gameObject, true);

        if (DeathDetect(targetX, targetZ))
        {
            return;
        }

        //郡似

        //Debug.Log(originX + "," + originZ + " " + CellParameter.CellInformation[originX, originZ].ObjectProperty.Hp);

        if (Mathf.Abs(originX - targetX) + Mathf.Abs(targetZ - targetZ) <= CellParameter.CellInformation[originX, originZ].ObjectProperty.BasicAttackDistance)//満議好似鉦宣譜崔葎0
        {
            attackTrueDamage = CellParameter.CellInformation[targetX, targetZ].ObjectProperty.HalfAttack();
            CellParameter.CellInformation[originX, originZ].ObjectProperty.BeBasicAttacked(attackTrueDamage[0], attackTrueDamage[1]);

            DeathDetect(originX, originZ);
        }

        //Debug.Log(originX + "," + originZ + " " + CellParameter.CellInformation[originX, originZ].ObjectProperty.Hp);

        StaticGameObject.UIDiceParentObject.SetActive(true);

        UnityEngine.Object.Destroy(gameObject);
    }

    //涙郡似噸好
    public void NonCounterBasicAttack(int originX, int originZ, int targetX, int targetZ, GameObject gameObject, bool goFromBasicAttackFunction = false)
    {
        //喘噐公黍繁俊辺議彬墾峙,0頁麗尖��1頁寔糞
        float[] attackTrueDamage = new float[2];

        attackTrueDamage = CellParameter.CellInformation[originX, originZ].ObjectProperty.BasicAttack();

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        CellParameter.CellInformation[targetX, targetZ].ObjectProperty.BeBasicAttacked(attackTrueDamage[0], attackTrueDamage[1]);

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        if (!goFromBasicAttackFunction)
        {
            DeathDetect(targetX, targetZ);
            StaticGameObject.UIDiceParentObject.SetActive(true);
            UnityEngine.Object.Destroy(gameObject);
        }
    }
    //泣方3！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！


    //棒蘭霞貌
    private bool DeathDetect(int cellX, int cellZ)
    {
        if (CellParameter.CellInformation[cellX, cellZ].ObjectProperty.DeathDetect())
        {
            cellFunction.DestroyCellObject(cellX, cellZ);

            PlayerParameter.Player[CellParameter.CellInformation[cellX, cellZ].PlayerIndex].MonsterGraveyardNum[CellParameter.CellInformation[cellX, cellZ].ObjectProperty.Level - 1]++;

            return true;
        }

        return false;
    }
}
