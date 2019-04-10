using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Camera 스크립트
/// </summary>
public class Camera : MonoBehaviour
{
    private GameObject player;

    // FIXME : 이벤트 주기 다시 짜야함.
    // GameManager에서 Instantiate 이후에만 load 가능해서 Start()에서 실행 안되는 것으로 추정.
    void FixedUpdate()
    {
        //Debug.Log(GameObject.FindGameObjectWithTag("Player"));
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        //if (player.transform.position.y > 0)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
    }
}
