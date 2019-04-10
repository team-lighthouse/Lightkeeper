using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    Vector3 StartingPos;
    Quaternion StartingRotate;
    static bool isEnded = false;

    // FIXME: sceneIdx. 현재는 scene 2 부터 되어있음.
    static int sceneIdx = 0;

    void Awake()
    {
        sceneIdx = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
        StartingPos = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        StartingRotate = GameObject.FindGameObjectWithTag("StartPoint").transform.rotation;

        Instantiate(player, StartingPos, StartingRotate);
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

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

    }

    public static void EndGame()
    {
        Time.timeScale = 0f;
        isEnded = true;
    }

}
