using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_444 : MonoBehaviour
{
    private int cellX, cellY;

    private bool flag_Attackable = false;//减少消耗 可以不进入update里的其他代码第二次

    private PlayerAbility playerAbility = new PlayerAbility();


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -ConstantParameter.diceHeight / 2f && !flag_Attackable)
        {
            cellX = (int)transform.position.x + 7;
            //如果是P1
            if (PlayerParameter.ActivePlayerIndex / 2 == (PlayerParameter.ActivePlayerIndex + 1) / 2)
            {
                cellY = (int)transform.position.z;
            }
            //如果是P2
            else
            {
                cellY = (int)transform.position.z - 495;
            }

            playerAbility.OriginCellX = cellX;
            playerAbility.OriginCellZ = cellY;

            //关掉UI骰子方便选择格子
            StaticGameObject.UIDiceParentObject.SetActive(false);

            playerAbility.PlayerAbilityAttack();

            //flag_Attackable = true;

            Destroy(gameObject);
        }
    }
}
