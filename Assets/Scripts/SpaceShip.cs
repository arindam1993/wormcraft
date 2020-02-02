using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{

    public ParticleSystem smoke;
    private float PercentageFixed = 0;
    private ParticleSystem.MainModule smoke_main;

    // Start is called before the first frame update
    void Start()
    {
        smoke = this.GetComponent<ParticleSystem>();
        smoke_main = smoke.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tools"))
        {
            this.addToFixed();
        }
    }

    public void addToFixed()
    {
        if (PercentageFixed <= 100)
        {
            PercentageFixed += 5;
        }
        smoke_main.startSize = smoke_main.startSize.constant - 0.1f;
    }

    public float GetPercentFixed()
    {
        return PercentageFixed;
    }
}
