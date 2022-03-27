using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility
{
    private const int  SpringIsComingRecoverDivisionRate = 2;

    private CellFunction cellFunction;

    private int originCellX, originCellZ;

    public int OriginCellX { get => originCellX; set => originCellX = value; }
    public int OriginCellZ { get => originCellZ; set => originCellZ = value; }

    public void PlayerAbilityAttack()
    {
        GetType().GetMethod(PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].PlayerAbilityName).Invoke(this,null);
    }

    //������ѡ��Ʒ
    public void Annihilate()
    {
        //���Ƿ������ͺ��ģ����߲�������
        string targetName = CellParameter.CellInformation[originCellX, originCellZ].Name;
        if (targetName.StartsWith("P1_") || targetName.StartsWith("P2_"))
        {
            //Debug.Log("���ɴݻ�");
            StaticGameObject.UIDiceParentObject.SetActive(true);
            return;
        }

        cellFunction.DestroyCellObject(originCellX, originCellZ);

        StaticGameObject.UIDiceParentObject.SetActive(true);
    }
    
    //���︴�գ����йֻ�Ѫ�������ֵ��50%
    public void SpringIsComing()
    {
        foreach(string name in PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name)
        {
            foreach(CellPosition cell in PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[name].MonsterCellIndexList)
            {

                //Debug.Log("���︴��");
                int hpMax = CellParameter.CellInformation[cell.X, cell.Z].ObjectProperty.HpMax;
                int hp = CellParameter.CellInformation[cell.X, cell.Z].ObjectProperty.Hp + hpMax / SpringIsComingRecoverDivisionRate;

                if (hp >= hpMax)
                    CellParameter.CellInformation[cell.X, cell.Z].ObjectProperty.Hp = hpMax;
                else
                    CellParameter.CellInformation[cell.X, cell.Z].ObjectProperty.Hp = hp;
            }

            
            StaticGameObject.UIDiceParentObject.SetActive(true);
        }
    }
}
