using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    Vector3 StartingPos;
    static bool isEnded = false;

    // FIXME: sceneIdx. 현재는 sampleScene 부터 되어있음.
    static int sceneIdx = 0;

    void Awake()
    {
        sceneIdx = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
        StartingPos = GameObject.FindGameObjectWithTag("StartPoint").transform.position;

        Instantiate(player, StartingPos, Quaternion.identity);
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label("STAGE INDEX " + sceneIdx);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        if (isEnded)
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("GAME END");

            if(sceneIdx<2 && GUILayout.Button ("GO NEXT"))
            {
                isEnded=false;
                sceneIdx++;
                SceneManager.LoadScene(sceneIdx, LoadSceneMode.Single);
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
