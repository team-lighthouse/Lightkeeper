using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene 전환, UI 처리
/// </summary>
public class GameManager : MonoBehaviour
{
    public static int sceneIndex = 0;
    void Awake()
    {
        // 현재 scene의 번호를 받아온다. scene index는 build setting에서 설정 가능.
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Game내 디버깅용 GUI
    /// </summary>
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label("STAGE INDEX " + sceneIndex);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

}
