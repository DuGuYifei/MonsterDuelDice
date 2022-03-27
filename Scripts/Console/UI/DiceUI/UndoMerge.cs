using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UndoMerge : MonoBehaviour,IPointerClickHandler
{
    public GameObject[] uiDice;
    public GameObject[] closeMergeButton_2;
    public GameObject[] openMergeButton;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameObject.SetActive(false);
            foreach (GameObject button in closeMergeButton_2)
            {
                button.SetActive(false);
            }
            foreach(GameObject ui in uiDice)
            {
                ui.SetActive(true);
            }
            if (ThrowDiceParameter.DiceThrowResult[0] == ThrowDiceParameter.DiceThrowResult[1] && ThrowDiceParameter.DiceThrowResult[0] == ThrowDiceParameter.DiceThrowResult[2])
            {
                //确认每个都没用过，都是新骰子
                bool flag_AllNew = true;
                foreach(bool element in ThrowDiceParameter.NewDice)
                {
                    if (!element)
                    {
                        flag_AllNew = false;
                        break;
                    }
                }

                //如果都是新的
                if (flag_AllNew)
                {
                    foreach (GameObject element in openMergeButton)
                    { 
                        element.SetActive(true); 
                    }
                    
                }
                //if (this.name == "UIDice_1_2" && ThrowDiceParameter.NewDice[2] && ThrowDiceParameter.NewDice[2])
                //{
                //    openMergeButton_13[0].SetActive(true);
                //    openMergeButton_13[1].SetActive(true);
                //    openMergeButton_13[2].SetActive(true);
                //}
                //else if (this.name == "UIDice_2_3" && ThrowDiceParameter.NewDice[0] && ThrowDiceParameter.NewDice[0])
                //{
                //    openMergeButton_13[0].SetActive(true);
                //    openMergeButton_13[1].SetActive(true);
                //    openMergeButton_13[2].SetActive(true);
                //}
            }
        }
    }
}
