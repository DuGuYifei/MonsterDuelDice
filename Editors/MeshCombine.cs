using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshCombine : EditorWindow
{

    static GameObject MeshTarget;
    static Material material;
    bool isCombine = false;

    [MenuItem("MyEditor/MeshCombineWindow")]
    private static void AddWindow2()
    {
        Rect _rect = new Rect(0, 0, 500, 500);
        MeshCombine window = (MeshCombine)EditorWindow.GetWindowWithRect(typeof(MeshCombine), _rect, true, "MeshCombine");
        window.Show();
    }

    private void OnGUI()
    {
        MeshTarget = EditorGUILayout.ObjectField("�����������", MeshTarget, typeof(GameObject), true) as GameObject;
        material = EditorGUILayout.ObjectField("���������ʾ����", material, typeof(Material), true) as Material;

        EditorGUILayout.BeginVertical();

        Color c = GUI.color;
        if (isCombine) GUI.color = Color.green;
        else GUI.color = Color.white;

        if (GUILayout.Button("�ϲ�mesh����", GUILayout.Width(200)))
        {
            MeshTarget.AddComponent<MeshFilter>();
            MeshTarget.AddComponent<MeshRenderer>();
            CombineMesh();
            isCombine = true;
        }
        GUI.color = c;
        EditorGUILayout.Space();
        if (GUILayout.Button("�رմ���", GUILayout.Width(200)))
        {
            //�رմ���
            this.Close();
        }
        EditorGUILayout.EndVertical();
    }

    static void CombineMesh()
    {

        MeshFilter[] meshFilters = MeshTarget.GetComponentsInChildren<MeshFilter>();
        //Debug.Log(meshFilters.ToString());
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh targetMesh = new Mesh();
        targetMesh.CombineMeshes(combineInstances);

        MeshTarget.GetComponent<MeshFilter>().sharedMesh = targetMesh;
        MeshTarget.GetComponent<Renderer>().material = material;
    }
}