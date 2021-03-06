﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    public static float BottomY = -15.0f;
    public static float TopY = 15.0f;

    public float RespawnTime;
    BaseDisableable disableTarget;
    bool isRespawning = false;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        disableTarget = this.gameObject.GetComponent<BaseDisableable>();
        player = this.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 center = disableTarget.GetCenter();
        if(center.y < BottomY)
        {
            if (isRespawning)
            {
                StartCoroutine(Respawn());
                StartCoroutine(RespawnTimer());
            }
            isRespawning = true;        
        }
    }


    IEnumerator Respawn()
    {
        disableTarget.WormcraftDisable();
        yield return new WaitForSeconds(RespawnTime);
        this.transform.position = new Vector3(Camera.main.transform.position.x, TopY, 0);
        disableTarget.WormcraftEnable();
        isRespawning = false;
        
        if(player != null)
        {
            yield return new WaitForSeconds(1.0f);
            GameManager0.Instance.respawnTimers[player.PlayerIndex].text = "";
        }
    }

    IEnumerator RespawnTimer()
    {
        if (player != null)
        {
            for (int i = (int)RespawnTime + 1; i >= 0; i--)
            {
                GameManager0.Instance.respawnTimers[player.PlayerIndex].text = i + "";
                yield return new WaitForSeconds(1.0f);
            }
            if (!isRespawning)
            {
                GameManager0.Instance.respawnTimers[player.PlayerIndex].text = "";
            }
        }
    }
}
