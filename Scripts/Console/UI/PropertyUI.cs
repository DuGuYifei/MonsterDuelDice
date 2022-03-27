using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyUI : MonoBehaviour
{
    private Text[] propertiesText;

    private CellFunction cellFunction = new CellFunction();

    private CellPosition cellPosition;

    private CanvasGroup properties;

    // Start is called before the first frame update
    void Start()
    {
        propertiesText = gameObject.GetComponentsInChildren<Text>();

        properties = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cellFunction.ClickCellPosition(out cellPosition))
        {
            if (CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty != null)
            {
                properties.alpha = 1;

                propertiesText[0].text = cellPosition.X + "," + cellPosition.Z;
                propertiesText[1].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].Name;
                propertiesText[2].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].PlayerIndex.ToString();
                propertiesText[3].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.Hp + "/" + CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.HpMax;
                propertiesText[4].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.AttackDamage.ToString();
                propertiesText[5].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.BasicAttackDistance.ToString();
                propertiesText[6].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.Armor.ToString();
                propertiesText[7].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.Mobility.ToString();

                foreach (var element in CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.DeBuff)
                {
                    if (element.Value > 0)
                    {
                        propertiesText[8].text = element.Value + " " + element.Key + "\n";
                    }
                }

                if (CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty is Monster)
                {
                    propertiesText[9].text = CellParameter.CellInformation[cellPosition.X, cellPosition.Z].ObjectProperty.AbilityAttackDistance.ToString();

                    propertiesText[10].gameObject.SetActive(true);
                }
                else
                {
                    propertiesText[10].gameObject.SetActive(false);
                }
            }

            //点击了空白就消失（必须点击了才行）
            else
                properties.alpha = 0;
        }

        else if(Input.GetMouseButtonDown(0))
        {
            properties.alpha = 0;
        }
    }
}
