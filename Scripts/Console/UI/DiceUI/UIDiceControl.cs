using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIDiceControl : MonoBehaviour
{
    private string[] dianshuPath = { "Picture/UIDice/dianshu01_1", 
                                     "Picture/UIDice/dianshu01_2", 
                                     "Picture/UIDice/dianshu01_3", 
                                     "Picture/UIDice/dianshu01_4", 
                                     "Picture/UIDice/dianshu01_5", 
                                     "Picture/UIDice/dianshu01_6" };

    public GameObject[] uiDice;                             //获得UI中几个dice的gameobject (1,2,3,1和2，2和3，1和2和3)（和表示合体)
    public GameObject[] uiDiceMerge;                        //获得让uidice融合的融合按钮(left1,right1,middle1,left2,right2,all)
    private Image[] imgOfUIDice = new Image[6];             //上面五个位置的骰子的image组件
    private Sprite[] spriteOfUIDice = new Sprite[6];        //1-6点数的sprite
    private bool goThrowing = false;                        //只能创建一次扔骰子的控制台，正在扔了就变成true，扔完了就变回false
    private bool nextRound = true;                         //防止同一个玩家不停扔骰子，按过R才能继续扔


    void Awake()
    {
        //初始化UI中骰子image的image组件，以及加载6个点数的精灵
        for (int i = 0; i < 6; i++)
        {
            imgOfUIDice[i] = uiDice[i].GetComponent<Image>();
            spriteOfUIDice[i]= Resources.Load<Sprite>(dianshuPath[i]);
        }
    }

    void Start()
    {
        //添加委托给UI下一回合开始的控制
        FindObjectOfType<RoundControl>().NextRound += UIDiceControl_NextRound;
    }

    void Update()
    {
        //按回车键开始掷色子的场景
        if (Input.GetKeyDown(KeyCode.Return) && !goThrowing && nextRound)
        {
            goThrowing = true;
            nextRound = false;

            string sceneName = "ConsoleThrowDice_P" + (PlayerParameter.ActivePlayerIndex + 1);

            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        //根据扔出的点数准备骰子
        if (ThrowDiceParameter.NewDice[0] && ThrowDiceParameter.NewDice[1] && ThrowDiceParameter.NewDice[2] && goThrowing)
        {
            goThrowing = false;
            StartCoroutine(UnLoadConsoleThrowDice());
        }
    }

    //扔完骰子后异步等待卸载扔骰子场景
    IEnumerator UnLoadConsoleThrowDice()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        string sceneName = "ConsoleThrowDice_P" + (PlayerParameter.ActivePlayerIndex + 1);

        SceneManager.UnloadSceneAsync(sceneName);

        //根据结果初始化ui上的骰子点数
        for (int i = 0; i < 3; i++)
        {
            PrepareUIDice(ThrowDiceParameter.DiceThrowResult[i], i);
        }
        InitMergeButton();
    }

    private void UIDiceControl_NextRound()
    {
        //回合结束时关闭当前骰子的各种UI
        for (int i = 0; i < 6; i++)
        {
            uiDice[i].SetActive(false);
            uiDiceMerge[i].SetActive(false);
        }

        ThrowDiceParameter.NewDice[0] = false;
        ThrowDiceParameter.NewDice[1] = false;
        ThrowDiceParameter.NewDice[2] = false;

        nextRound = true;
    }

    //给UI骰子贴上点数精灵
    private void PrepareUIDice(int point, int IndexUIDice)
    {       
        imgOfUIDice[IndexUIDice].sprite = spriteOfUIDice[point - 1];
        uiDice[IndexUIDice].SetActive(true);
    }

    //激活融合按键
    private void InitMergeButton()
    {
        if (ThrowDiceParameter.DiceThrowResult[0] == ThrowDiceParameter.DiceThrowResult[1])
        {
            uiDiceMerge[0].SetActive(true);
            if(ThrowDiceParameter.DiceThrowResult[1] == ThrowDiceParameter.DiceThrowResult[2])
            {
                uiDiceMerge[1].SetActive(true);
                uiDiceMerge[2].SetActive(true);
                uiDiceMerge[5].SetActive(true);

                return;
            }
        }
        else if(ThrowDiceParameter.DiceThrowResult[0] == ThrowDiceParameter.DiceThrowResult[2])
        {
            uiDiceMerge[2].SetActive(true);
            return;
        }
        else if (ThrowDiceParameter.DiceThrowResult[1] == ThrowDiceParameter.DiceThrowResult[2])
        {

            uiDiceMerge[1].SetActive(true);
        }
    }


    //点击融合按钮触发的函数
    //最后要将静态扔骰子的结果里用不到的参数变为0,融合的均取第二个值
    private void MergeLeft_1()
    {
        uiDiceMerge[0].SetActive(false);
        uiDice[0].SetActive(false);
        uiDice[1].SetActive(false);
        PrepareUIDice(ThrowDiceParameter.DiceThrowResult[1], 3);
        //如果三个都行等//且都是新骰子
        if (ThrowDiceParameter.DiceThrowResult[0]==ThrowDiceParameter.DiceThrowResult[2] && ThrowDiceParameter.NewDice[2])
        {     
            DisableFirstMerge();
            uiDiceMerge[4].SetActive(true);
        }
    }
    private void MergeRight_1()
    {
        uiDiceMerge[1].SetActive(false);
        uiDice[1].SetActive(false);
        uiDice[2].SetActive(false);
        PrepareUIDice(ThrowDiceParameter.DiceThrowResult[1], 4);
        //如果三个都行等
        if (ThrowDiceParameter.DiceThrowResult[0] == ThrowDiceParameter.DiceThrowResult[2] && ThrowDiceParameter.NewDice[0])
        {
            DisableFirstMerge();
            uiDiceMerge[3].SetActive(true);
        }
    }
    private void MergeMiddle_1()
    {
        uiDiceMerge[2].SetActive(false);
        //1 3相等，就先把2 3换位置变成mergeleft1
        ThrowDiceParameter.DiceThrowResult[2] = ThrowDiceParameter.DiceThrowResult[1];
        ThrowDiceParameter.DiceThrowResult[1] = ThrowDiceParameter.DiceThrowResult[0];
        if(ThrowDiceParameter.NewDice[1]!= ThrowDiceParameter.NewDice[2])
        {
            ThrowDiceParameter.NewDice[1] = !ThrowDiceParameter.NewDice[1];
            ThrowDiceParameter.NewDice[2] = !ThrowDiceParameter.NewDice[2];
        }

        uiDice[2].SetActive(false);
        if (ThrowDiceParameter.NewDice[2])
        {
            PrepareUIDice(ThrowDiceParameter.DiceThrowResult[2], 2);
        }

        PrepareUIDice(ThrowDiceParameter.DiceThrowResult[1], 1);
        uiDice[0].SetActive(false);
        uiDice[1].SetActive(false);
        PrepareUIDice(ThrowDiceParameter.DiceThrowResult[1], 3);

        //如果三个都行等
        if (ThrowDiceParameter.DiceThrowResult[0] == ThrowDiceParameter.DiceThrowResult[2])
        {
            DisableFirstMerge();
            if (ThrowDiceParameter.NewDice[2])
            {
                uiDiceMerge[4].SetActive(true);
            }
        }
    }
    private void MergeLeft_2()
    {
        uiDiceMerge[3].SetActive(false);
        uiDice[0].SetActive(false);
        uiDice[4].SetActive(false);
        PrepareUIDice(ThrowDiceParameter.DiceThrowResult[1], 5);
    }
    private void MergeRight_2()
    {
        uiDiceMerge[4].SetActive(false);
        uiDice[3].SetActive(false);
        uiDice[2].SetActive(false);
        PrepareUIDice(ThrowDiceParameter.DiceThrowResult[1], 5);
    }
    private void MergeAll_3()
    {
        uiDice[0].SetActive(false);
        uiDice[1].SetActive(false);
        uiDice[2].SetActive(false);
        DisableFirstMerge();
        PrepareUIDice(ThrowDiceParameter.DiceThrowResult[1], 5);
    }
    private void DisableFirstMerge()
    {
        foreach (int i in new int[] { 0, 1, 2, 5 })
        {
            uiDiceMerge[i].SetActive(false);
        }
    }
}