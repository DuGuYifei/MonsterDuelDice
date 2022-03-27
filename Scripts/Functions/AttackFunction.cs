using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFunction
{
    private CellFunction cellFunction;

    //¹¥»÷ÓÃº¯Êý
    //Ñ°ÕÒµÐÈË
    public bool FindEnemy(List<CellPosition> scope, int currentPlayerIndex)
    {
        foreach (CellPosition cell in scope)
        {
            if (CellParameter.CellInformation[cell.X, cell.Z].PlayerIndex != -1 && CellParameter.CellInformation[cell.X, cell.Z].PlayerIndex != currentPlayerIndex)
                return true;
        }

        return false;
    }


    //µãÊý4£º¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
    //µãÊý4ºÍ44Í¨ÓÃ£º¼¼ÄÜ¹¥»÷
    public void AbilityAttack(int originX, int originZ, int targetX, int targetZ, GameObject gameObject)
    {
        //ÓÃÓÚ¸øµÐÈË½ÓÊÕµÄÉËº¦Öµ,0ÊÇÎïÀí£¬1ÊÇÕæÊµ
        float[] attackTrueDamage = new float[2];

        attackTrueDamage = CellParameter.CellInformation[originX, originZ].ObjectProperty.AbilityAttack();

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        CellParameter.CellInformation[targetX, targetZ].ObjectProperty.BeAbilityAttacked(attackTrueDamage[0], attackTrueDamage[1]);

        //Debug.Log(targetX + "," + targetZ + " " + CellParameter.CellInformation[targetX, targetZ].ObjectProperty.Hp);

        DeathDetect(targetX, targetZ);

        StaticGameObject.UIDiceParentObject.SetActive(true);
        UnityEngine.Object.Destroy(gameObject);
    }
    //µãÊý4£º¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª



    //µãÊý3£º¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
    //Á¬ÐøÁ½´ÎÆÕ¹¥
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

    //ÆÕ¹¥
    public void BasicAttack(int originX, int originZ, int targetX, int targetZ, GameObject gameObject)
    {
        //ÓÃÓÚ¸øµÐÈË½ÓÊÕµÄÉËº¦Öµ,0ÊÇÎïÀí£¬1ÊÇÕæÊµ
        float[] attackTrueDamage = new float[2];

        NonCounterBasicAttack(originX, originZ, targetX, targetZ, gameObject, true);

        if (DeathDetect(targetX, targetZ))
        {
            return;
        }

        //·´»÷

        //Debug.Log(originX + "," + originZ + " " + CellParameter.CellInformation[originX, originZ].ObjectProperty.Hp);

        if (Mathf.Abs(originX - targetX) + Mathf.Abs(targetZ - targetZ) <= CellParameter.CellInformation[originX, originZ].ObjectProperty.BasicAttackDistance)//ËþµÄ¹¥»÷¾àÀëÉèÖÃÎª0
        {
            attackTrueDamage = CellParameter.CellInformation[targetX, targetZ].ObjectProperty.HalfAttack();
            CellParameter.CellInformation[originX, originZ].ObjectProperty.BeBasicAttacked(attackTrueDamage[0], attackTrueDamage[1]);

            DeathDetect(originX, originZ);
        }

        //Debug.Log(originX + "," + originZ + " " + CellParameter.CellInformation[originX, originZ].ObjectProperty.Hp);

        StaticGameObject.UIDiceParentObject.SetActive(true);

        UnityEngine.Object.Destroy(gameObject);
    }

    //ÎÞ·´»÷ÆÕ¹¥
    public void NonCounterBasicAttack(int originX, int originZ, int targetX, int targetZ, GameObject gameObject, bool goFromBasicAttackFunction = false)
    {
        //ÓÃÓÚ¸øµÐÈË½ÓÊÕµÄÉËº¦Öµ,0ÊÇÎïÀí£¬1ÊÇÕæÊµ
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
    //µãÊý3¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª


    //ËÀÍö²âËÆ
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
