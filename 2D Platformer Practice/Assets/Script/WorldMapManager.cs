using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// world map manager, 장기적으로 scene manager 와 통합 할 계획임.
/// </summary>
public class WorldMapManager : MonoBehaviour
{
    // public void OnGUI()
    // {
    //     GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
    //     GUILayout.BeginHorizontal();
    //     GUILayout.FlexibleSpace();

    //     GUILayout.Label("WORLD MAP");

    //     GUILayout.EndHorizontal();
    //     GUILayout.EndArea();

    // }

    /// <summary>
    /// 월드맵의 버튼을 처리하는 함수
    /// </summary>
    public void btnClick()
    {
        Debug.Log("BUTTON NAME: " + EventSystem.current.currentSelectedGameObject.name);

        char sp = '_';
        string[] splited = EventSystem.current.currentSelectedGameObject.name.Split(sp);
        Debug.Log("Go To Scene :" + splited[1]);

        SceneManager.LoadScene(int.Parse(splited[1]), LoadSceneMode.Single);
    }
}