using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


/// <summary>
/// Scene 전환, UI 처리
/// </summary>
public class GameManager : MonoBehaviour
{
    public static int sceneIndex = 0;
    bool pause = false;

    GameObject pauseBtn, chkPointBtn, mapBtn, resumeBtn;



    void Awake()
    {
        // 현재 scene의 번호를 받아온다. scene index는 build setting에서 설정 가능.
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex != 0)
        {
            // GameObject.Find 가 inactive gameobject를 받아오지 못해서 넣은 코드.
            pauseBtn = GameObject.Find("PauseBtn");
            chkPointBtn = GameObject.Find("GoToPointBtn");
            mapBtn = GameObject.Find("GoToMapBtn");
            resumeBtn = GameObject.Find("ResumeBtn");

            chkPointBtn.SetActive(false);
            mapBtn.SetActive(false);
            resumeBtn.SetActive(false);
        }
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

        if(pause) // 일시정지 화면
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// 월드맵의 버튼을 처리하는 함수
    /// </summary>
    public void WorldMapBtnClick()
    {
        Debug.Log("BUTTON NAME: " + EventSystem.current.currentSelectedGameObject.name);

        char sp = '_';
        string[] splited = EventSystem.current.currentSelectedGameObject.name.Split(sp);
        Debug.Log("Go To Scene :" + splited[1]);

        SceneManager.LoadScene(int.Parse(splited[1]), LoadSceneMode.Single);
    }

    // 일시정지 기능 구현하기
    public void PauseBtnClick()
    {
        pause = true;
        
        chkPointBtn.SetActive(true);
        mapBtn.SetActive(true);
        resumeBtn.SetActive(true);
        pauseBtn.SetActive(false);
    }

    public void GoToMapBtnClick()
    {
        chkPointBtn.SetActive(false);
        mapBtn.SetActive(false);
        resumeBtn.SetActive(false);
        pauseBtn.SetActive(false);

        SceneManager.LoadScene(0, LoadSceneMode.Single);

        pause = false;
    }

    public void GoToPointBtnClick()
    {
        // TODO: not yet implemented
        chkPointBtn.SetActive(false);
        mapBtn.SetActive(false);
        resumeBtn.SetActive(false);
        pauseBtn.SetActive(true);

        InStageManager ISM = GameObject.Find("Managers").GetComponent<InStageManager>();
        // Debug.LogError(ISM);
        ISM.returnCheckPoint();

        pause = false;
    }

    public void ResumeBtnClick()
    {
        chkPointBtn.SetActive(false);
        mapBtn.SetActive(false);
        resumeBtn.SetActive(false);
        pauseBtn.SetActive(true);

        pause = false;
    }

}
