using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 스테이지 내부에서 일어나는 일들을 처리하는 매니저.
/// 스테이
/// </summary>
public class InStageManager : MonoBehaviour
{
    public GameObject player;
    public static Vector3 StartingPos;
    public static bool isEnded = false;

    public bool resetCheckPoint = false;


    void Awake()
    {
        // 테스트 용으로 reset 옵션을 켜고 실행하면 playerprefs에 저장된 체크포인트를 삭제한다.
        if(resetCheckPoint)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void Start()
    {
        if(PlayerPrefs.HasKey("CheckPoint_" + GameManager.sceneIndex.ToString()))
        {
            int CheckPointIdx = PlayerPrefs.GetInt("CheckPoint_" + GameManager.sceneIndex.ToString());
            Debug.Log("SCENE Index: " + GameManager.sceneIndex.ToString());
            Debug.Log("CHECK POINT Index: " + CheckPointIdx.ToString());
            StartingPos = GameObject.Find("CheckPoint_" + GameManager.sceneIndex.ToString() + "_" +CheckPointIdx.ToString()).transform.position;
        }
        else
        {
            StartingPos = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        }

        Instantiate(player, StartingPos, Quaternion.identity);
    }

    void OnGUI()
    {
        if (isEnded)
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("GAME END");

            if (GameManager.sceneIndex < 2 && GUILayout.Button("GO NEXT"))
            {
                isEnded = false;

                // FIXME: 현재는 단순히 다음 scene loading, 월드맵으로 load해야함.
                // sceneIndex++;
                // SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }

    public static void EndGame()
    {
        isEnded = true;
    }
}