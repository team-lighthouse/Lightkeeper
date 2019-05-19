using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_body : MonoBehaviour
{
    public bool bodyHit;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "CheckPoint")
        {
            char sp = '_';
            string[] splited = collision.gameObject.name.Split(sp);
            Debug.Log("Splited :" + " 0: " + splited[0] + " 1: " + splited[1] + " 2: "+splited[2]);
            int newIndex = int.Parse(splited[2]);

            if(PlayerPrefs.HasKey("CheckPoint_" + GameManager.sceneIndex.ToString()))
            {
                int storedIndex = PlayerPrefs.GetInt("CheckPoint_" + GameManager.sceneIndex.ToString());
                if(storedIndex != newIndex) // 다른 체크포인트일 경우 기존 올라온 깃발을 다시 내린다.
                {
                    GameObject go = GameObject.Find("CheckPoint_" + GameManager.sceneIndex.ToString() + "_" +storedIndex.ToString()); // 기존 저장된 정보 찾는다.
                    SpriteRenderer srOld = go.GetComponent<SpriteRenderer>();
                    Sprite oldFlag = Resources.Load <Sprite>("Sprite/flag_off");
                    srOld.sprite = oldFlag;
                }
            }
            
            PlayerPrefs.SetInt("CheckPoint_" + GameManager.sceneIndex, newIndex);
            InStageManager.StartingPos = collision.gameObject.transform.position;
            
            /// 아래는 새 이미지로 교체.
            SpriteRenderer srNew = collision.gameObject.GetComponent<SpriteRenderer>();
            Sprite newFlag = Resources.Load <Sprite>("Sprite/flag_on");
            srNew.sprite = newFlag;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
