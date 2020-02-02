using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    public static float BottomY = -15.0f;
    public static float TopY = 6.0f;

    public float RespawnTime;
    BaseDisableable disableTarget;
    bool isRespawning = false;

    // Start is called before the first frame update
    void Start()
    {
        disableTarget = this.gameObject.GetComponent<BaseDisableable>();
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
    }
}
