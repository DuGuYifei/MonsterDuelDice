using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDice : MonoBehaviour
{
    private const float constDiceTorqueForce = 10;       //扔骰子的扭矩
    private const float constDiceThrowForceZ = 20;       //扔骰子的z轴力度
    private const float constDiceThrowForceY = -5;       //扔骰子的y轴力度

    private Rigidbody dice;                         //骰子的刚体                    

    private int throwDice;                          //骰子只能扔一次,扔完变成1，测试完是否停下变成2
    private int diceThrowResult;                    //骰子的点数

    // Start is called before the first frame update
    void Start()
    {
        throwDice = 0;
        //diceThrowResult = 1;                         //初始化方便后面比较得出点数
        dice = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && throwDice == 0)
        {
            throwDice = 1;

            dice.useGravity = true;             //开启重力

            float x = Random.Range(0, 90);      //骰子旋转的欧拉角
            float y = Random.Range(0, 90);
            float z = Random.Range(0, 90);
            float diceThrowForceX = Random.Range(-8,8);              //扔骰子的x轴力度

            float diceThrowForceZ;
            float diceTorqueForce;

            //如果是P1
            if (PlayerParameter.ActivePlayerIndex / 2 == (PlayerParameter.ActivePlayerIndex + 1) / 2)
            {
                diceThrowForceZ = constDiceThrowForceZ;
                diceTorqueForce = constDiceTorqueForce;
            }
            //如果是P2
            else
            {
                diceThrowForceZ = -constDiceThrowForceZ;
                diceTorqueForce = -constDiceTorqueForce;
            }

            transform.Rotate(x, y, z);
            dice.AddTorque(Vector3.left * diceTorqueForce, ForceMode.Impulse);
            dice.AddForce(diceThrowForceX, constDiceThrowForceY, diceThrowForceZ, ForceMode.Impulse);
        }

        if (throwDice == 1)
        {
            if (dice.IsSleeping())
            {
                DiceNumber();
                throwDice = 2;           
            }
        }
    }

    //得到扔骰子的结果
    private void DiceNumber()
    {
        //用骰子自带的前上右向量求哪个面朝上，比较骰子六个面的y轴大小用，最大的朝上
        float[] yOfDiceFaces = new float[6] 
        { 
            transform.forward.y,        //1
            transform.right.y*(-1),     //2
            transform.up.y,             //3
            transform.up.y*(-1),        //4
            transform.right.y,          //5
            transform.forward.y*(-1)    //6 
        };
        float medium = yOfDiceFaces[0];
        diceThrowResult = 1;
        for (int i=1; i<6; i++)
        {
            if(yOfDiceFaces[i]>medium)
            {
                medium = yOfDiceFaces[i];
                diceThrowResult = i + 1;
            }
        }
        //将获得的骰子值发给controller
        switch (this.name)
        {
            case "Dice1":
                //float rate = Random.value;
                //if(rate>0.5f)
                ThrowDiceParameter.DiceThrowResult[0] =
                    diceThrowResult;
                    //2;
                //else
                //{
                //    ThrowDiceParameter.DiceThrowResult[0] =
                //    //diceThrowResult;
                //    2;
                //}
                ThrowDiceParameter.NewDice[0] = true;
                break;
            case "Dice2":
                ThrowDiceParameter.DiceThrowResult[1] =
                    diceThrowResult;
                    //2;
                ThrowDiceParameter.NewDice[1] = true;
                break;
            case "Dice3":
                ThrowDiceParameter.DiceThrowResult[2] =
                    diceThrowResult;
                    //2;
                ThrowDiceParameter.NewDice[2] = true;
                break;
        }
    }
}
