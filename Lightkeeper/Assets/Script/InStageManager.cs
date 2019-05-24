using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// 스테이지 내부에서 일어나는 일들을 처리하는 매니저.
/// </summary>
public class InStageManager : MonoBehaviour
{
    public GameObject player;
    public static Vector3 StartingPos;
    public static bool isEnded = false;

    public bool resetCheckPointAndCoin = false;

    int treeLimit;
    public Queue<GameObject> trees = new Queue<GameObject>();

    List<GameObject> tempCoins = new List<GameObject>();

    void Awake()
    {
        // 테스트 용으로 reset 옵션을 켜고 실행하면 playerprefs에 저장된 체크포인트를 삭제한다.
        if(resetCheckPointAndCoin)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void Start()
    {
        Debug.Log("SCENE IDX: " + GameManager.sceneIndex.ToString());
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

        treeLimit = 3;

        if(PlayerPrefs.HasKey("Coin_"+GameManager.sceneIndex.ToString()))
        {
            int[] savedCoin = PlayerPrefsX.GetIntArray("Coin_"+GameManager.sceneIndex.ToString());
            // foreach(int i in savedCoin)
            // {
            //     Debug.Log("SAVED COIN INFO: " + i);
            // }
            foreach(int i in savedCoin)
            {
                var earnedCoin = GameObject.Find("Coin_"+GameManager.sceneIndex.ToString()+"_"+i.ToString());
                earnedCoin?.SetActive(false);
            }
        }

    }

    void Update()
    {
        if (trees.Count > treeLimit)
        {
            Destroy(trees.Dequeue());
        }
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

    // 체크포인트로 되돌아감.
    public void returnCheckPoint()
    {
        Debug.Log("return");
        GameObject.FindGameObjectWithTag("Player").transform.position = StartingPos + new Vector3(0, 0.5f, 0);
        
        foreach(GameObject coin in tempCoins)
        {
            Debug.Log("RETURN COIN INFO: " + coin);
            coin.SetActive(true);
        }
    }

    public void chkCheckPoint()
    {

    }

    public void handleCoin(GameObject coin)
    {
        tempCoins.Add(coin);

        coin.SetActive(false);
    }

    public void saveCoin()
    {
        int[] savedCoin = PlayerPrefsX.GetIntArray("Coin_"+GameManager.sceneIndex.ToString());
        List<int> saved = savedCoin.ToList();
        List<int> tempCoin = new List<int>();
        // foreach(int i in saved)
        // {
        //     Debug.Log("Already SAVED COIN GAMEOBJECT INFO: " + i);
        // }
        foreach(GameObject coin in tempCoins)
        {
            char sp = '_';
            string[] splited = coin.name.Split(sp);
            Debug.Log("Coin Splited :" + " 0: " + splited[0] + " 1: " + splited[1] + " 2: " + splited[2]);
            int newIndex = int.Parse(splited[2]);

            tempCoin.Add(newIndex);
        }
        // foreach(int i in tempCoin)
        // {
        //     Debug.Log("TEMP COIN GAMEOBJECT INFO: " + i);
        // }
        List<int> renewCoin = saved.Concat(tempCoin).ToList();
        // foreach(int i in renewCoin)
        // {
        //     Debug.Log("SAVING COIN INFO: " + i);
        // }
        PlayerPrefsX.SetIntArray("Coin_"+GameManager.sceneIndex.ToString(), renewCoin.ToArray());
        
        tempCoin.Clear();
        tempCoins.Clear();
    }
}