﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : BaseDisableable
{

    public ParticleSystem smoke;
    private float PercentageFixed = 0;
    private ParticleSystem.MainModule smoke_main;

    public static SpaceShip Instance;

    void Awake()
    {
        Instance = this;
    }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Tools"))
        {
            this.addToFixed();
        }
        Destroy(collision.gameObject);
    }

    public void addToFixed()
    {
        if (PercentageFixed <= 100)
        {
            PercentageFixed += 10;
        }
        smoke_main.startSize = smoke_main.startSize.constant - 0.1f;
    }

    public float GetPercentFixed()
    {
        return PercentageFixed;
    }

    public override void WormcraftDisable()
    {
        base.WormcraftDisable();
        this.GetComponent<Rigidbody2D>().simulated = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void WormcraftEnable()
    {
        base.WormcraftEnable();
        this.GetComponent<Rigidbody2D>().simulated = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        this.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;

    }
}
