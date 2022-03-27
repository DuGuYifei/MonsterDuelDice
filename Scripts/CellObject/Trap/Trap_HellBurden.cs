using UnityEngine;
/// <summary>
/// �����ظ�
/// ��������ÿ���ƶ��ĸ����� ���� ����Ч��
/// </summary>
public class Trap_HellBurden : Trap
{
    private TrapCellFunction trapCellFunction = new TrapCellFunction();

    public void Explode(int targetX, int targetY)
    {
        //���������ƶ�����
        CellParameter.CellInformation[targetX, targetY].ObjectProperty.Mobility -= 2;

        //Debug.Log("�ƶ�������" + CellParameter.CellInformation[targetX, targetY].ObjectProperty.Mobility);

        //�������غ�
        CellParameter.CellInformation[targetX, targetY].ObjectProperty.DeBuff["retard"] += 3;

        trapCellFunction.DestroyTrapObject(targetX, targetY);
    }
}
