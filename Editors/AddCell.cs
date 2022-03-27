using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.Text;

public class WindowAddCell : EditorWindow
{
    GameObject fatherCheckerboard;
    private string scaleString;
    private float scaleValue;
    //打开自定义窗口
    [MenuItem("MyEditor/Cell/AddCell")]
    public static void Window()
    {
        EditorWindow.GetWindow(typeof(WindowAddCell));
    }

    //设置窗口名字
    WindowAddCell()
    {
        this.titleContent = new GUIContent("Add Cell");
    }

    //绘制窗口界面
    private void OnGUI()
    {
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Add Cell");

        //显示当前场景
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUILayout.Label("Currently Scene: " + EditorSceneManager.GetActiveScene().name);

        //绘制gameobject
        GUILayout.Space(10);
        fatherCheckerboard = (GameObject)EditorGUILayout.ObjectField("Father Checkerboard", fatherCheckerboard, typeof(GameObject), true);

        //绘制文本
        GUILayout.Space(10);
        scaleString = EditorGUILayout.TextField("scale float", scaleString);
        scaleValue = Convert.ToSingle(scaleString);

        //添加按钮
        GUILayout.Space(10);
        if (GUILayout.Button("Add Cell"))
        {
            AddCell();
        }
    }

    private void AddCell()
    {
        for (float i = 0.5f; i <= 15f; i++)
        {
            for (float j = 0.5f; j <= 15f; j++)
            {
                GameObject empty = ObjectFactory.CreateGameObject("Cell" + Convert.ToString((int)j) + "_" + Convert.ToString((int)i), typeof(CellInfo));
                //这里需要更改typeof的类
                empty.transform.SetParent(fatherCheckerboard.transform);
                empty.transform.localPosition = new Vector3(j * scaleValue, 0f, i * scaleValue);
            }
        }
    }
}
