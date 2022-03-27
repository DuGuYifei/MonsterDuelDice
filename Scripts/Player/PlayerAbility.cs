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

    //湮灭所选物品
    public void Annihilate()
    {
        //不是防御塔和核心，二者不可湮灭
        string targetName = CellParameter.CellInformation[originCellX, originCellZ].Name;
        if (targetName.StartsWith("P1_") || targetName.StartsWith("P2_"))
        {
            //Debug.Log("不可摧毁");
            StaticGameObject.UIDiceParentObject.SetActive(true);
            return;
        }

        cellFunction.DestroyCellObject(originCellX, originCellZ);

        StaticGameObject.UIDiceParentObject.SetActive(true);
    }
    
    //万物复苏：所有怪回血最大生命值的50%
    public void SpringIsComing()
    {
        foreach(string name in PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].Monster_S_Name)
        {
            foreach(CellPosition cell in PlayerParameter.Player[PlayerParameter.ActivePlayerIndex].MonsterDict[name].MonsterCellIndexList)
            {

                //Debug.Log("万物复苏");
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
