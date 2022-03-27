using UnityEngine;
/// <summary>
/// 地狱重负
/// 怪物会减少每次移动的格子数 属于 负面效果
/// </summary>
public class Trap_HellBurden : Trap
{
    private TrapCellFunction trapCellFunction = new TrapCellFunction();

    public void Explode(int targetX, int targetY)
    {
        //减少两格移动能力
        CellParameter.CellInformation[targetX, targetY].ObjectProperty.Mobility -= 2;

        //Debug.Log("移动能力是" + CellParameter.CellInformation[targetX, targetY].ObjectProperty.Mobility);

        //持续三回合
        CellParameter.CellInformation[targetX, targetY].ObjectProperty.DeBuff["retard"] += 3;

        trapCellFunction.DestroyTrapObject(targetX, targetY);
    }
}
