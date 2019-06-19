using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

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
    
    bool isCleared = false;
    bool firstTime = false;
    public int beaconCount = 0;

    GameObject ClearText, ClearCoin;


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

        treeLimit = 3;

        if(PlayerPrefs.HasKey("Coin_"+GameManager.sceneIndex.ToString()))
        {
            int[] savedCoin = PlayerPrefsX.GetIntArray("Coin_"+GameManager.sceneIndex.ToString());
            // foreach(int i in savedCoin)
            // {
            //     Debug.Log("SAVED COIN INFO: " + i);
            // }

            int coinIdx = 0;

            foreach(int i in savedCoin)
            {
                var earnedCoin = GameObject.Find("Coin_"+GameManager.sceneIndex.ToString()+"_"+i.ToString());
                earnedCoin?.SetActive(false);
                coinIdx++;
            }

            GameObject CoinText = GameObject.Find("CoinText");
            coinIdx /= 2;
            CoinText.GetComponent<Text>().text = "X " + coinIdx.ToString();
        }

        if(PlayerPrefs.HasKey("Beacon_"+GameManager.sceneIndex.ToString()))
        {
            beaconCount = PlayerPrefs.GetInt("Beacon_"+GameManager.sceneIndex.ToString());
        }

        if(PlayerPrefs.HasKey("Clear_"+GameManager.sceneIndex.ToString()))
        {
            ClearText = GameObject.Find("ClearText");
            ClearCoin = GameObject.Find("ClearCoin");
            ClearText.SetActive(false);
            ClearCoin.SetActive(false);

            isCleared = true;
            StartingPos = GameObject.FindGameObjectWithTag("EndPoint").transform.position;

            SpriteRenderer srNew = GameObject.FindGameObjectWithTag("Beacon").gameObject.GetComponent<SpriteRenderer>();
            Sprite BeaconOn = Resources.Load<Sprite>("Sprite/beacon_on");
            srNew.sprite = BeaconOn;
        }
        else
        {
            ClearText = GameObject.Find("ClearText");
            ClearCoin = GameObject.Find("ClearCoin");
            ClearText.SetActive(false);
            ClearCoin.SetActive(false);
        }

        Instantiate(player, StartingPos, Quaternion.identity);

        if(GameManager.sceneIndex == 1)
        {
            GameObject welcomeText = GameObject.Find("WelcomeText");

            if (PlayerPrefs.HasKey("CheckPoint_1"))
            {
                welcomeText.SetActive(false);
            }
            else
            {
                StartCoroutine(disable(welcomeText, 5.0f, false));
            }
        }

    }

    IEnumerator disable(GameObject go, float waitTime, bool isCleared){ 
        yield return new WaitForSeconds(waitTime); 
        go.SetActive(false); 
        if(isCleared)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    } 

    void Update()
    {
        // Tree 개수 제한
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
        removeTrees();
        Debug.Log("return");
        GameObject.FindGameObjectWithTag("Player").transform.position = StartingPos + new Vector3(0, 0.5f, 0);
        
        foreach(GameObject coin in tempCoins)
        {
            Debug.Log("RETURN COIN INFO: " + coin);
            coin.SetActive(true);
        }
        tempCoins.Clear();
    }

    public void chkCheckPoint()
    {

    }

    public void handleCoin(GameObject coin)
    {
        foreach (GameObject go in tempCoins)
        {
            if(go == coin) {return;}
        }
        tempCoins.Add(coin);

        Debug.Log("HANDLE COIN: "+ coin);

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
        
        int coinIdx = 0;

        foreach(int i in renewCoin)
        {
            coinIdx++;
        }

        GameObject CoinText = GameObject.Find("CoinText");
        CoinText.GetComponent<Text>().text = "X " + coinIdx.ToString();

        tempCoin.Clear();
        tempCoins.Clear();
    }

    public void removeTrees()
    {
        while (trees.Count > 0)
        {
            Destroy(trees.Dequeue());
        }
    }

    public void beaconHit()
    {
        beaconCount++;
        if(beaconCount == 5)
        {
            clearStage();
            PlayerPrefs.SetInt("Beacon_"+GameManager.sceneIndex.ToString(), beaconCount);
        }
    }

    public void clearStage()
    {
        isCleared = true;
        saveCoin();
        PlayerPrefs.SetInt("Clear_"+GameManager.sceneIndex.ToString(), 1);
        ClearText.SetActive(true);

        int[] savedCoin = PlayerPrefsX.GetIntArray("Coin_"+GameManager.sceneIndex.ToString());
            // foreach(int i in savedCoin)
            // {
            //     Debug.Log("SAVED COIN INFO: " + i);
            // }

            int coinIdx = 0;

            foreach(int i in savedCoin)
            {
                coinIdx++;
            }

            int[] MaxCoin = new int[] {10, 9, 9, 10};

            ClearCoin.GetComponent<Text>().text = coinIdx.ToString() + " / " + MaxCoin[GameManager.sceneIndex - 1];
        
            ClearCoin.SetActive(true);


        StartCoroutine(disable(ClearText, 3.0f, true));
    }
}