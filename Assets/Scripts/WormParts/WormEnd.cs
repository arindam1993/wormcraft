﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnd : MonoBehaviour
{
    private Rigidbody2D grabbableObject;

    //This joint is dynmically created in order to grab things
    private FixedJoint2D grabJoint;

    private int layerMask;
    private Rigidbody2D rbd;
    private SpriteRenderer rend;
    private GameObject otherEnd;

    public float grabRadius;
    public Bounds displayBounds;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.NameToLayer("Grabbable");
        rbd = this.GetComponent<Rigidbody2D>();
        rend = this.GetComponent<SpriteRenderer>();
        displayBounds = rend.bounds;
        string otherName = this.gameObject.name == "End_A" ? "End_B" : "End_A";
        otherEnd = this.transform.parent.Find(otherName).gameObject;
    }

    private void FixedUpdate()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(this.transform.position, grabRadius, 1 << layerMask);
        Collider2D overlap = null;
        float minDistance = 999999.0f;
        //Find closest object that is not myself and the other end of thesame worm
        foreach( Collider2D overlapped in overlaps)
        {
            if(overlapped.gameObject != this.gameObject && overlapped.gameObject != otherEnd)
            {
                float distance = Vector2.Distance(this.transform.position, overlapped.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    overlap = overlapped;
                }
            }
        }

        if(overlap != null)
        {
            grabbableObject = overlap.gameObject.GetComponent<Rigidbody2D>();
        }
        else
        {
            grabbableObject = null;
        }
    }

    private void Update()
    {
        if (!IsGrabbing())
        {
            rend.color = Color.white;
        }
        else
        {
            rend.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        }

        displayBounds = rend.bounds;
    }

    public void Grab()
    {
        if(grabbableObject != null && this.grabJoint == null)
        {
            //Grab by creating a fixed joint between this end and the grabbable object
            grabJoint = this.gameObject.AddComponent<FixedJoint2D>();
            grabJoint.connectedBody = grabbableObject;


            AudioManager.Instance.PlayMagnetFX();
        }
    }

    public void Release()
    {
        if(this.grabJoint != null)
        {
            this.grabJoint.connectedBody = null;
            Component.Destroy(this.grabJoint);
            this.grabJoint = null;

            AudioManager.Instance.PlayThrowFX();
        }
    }

    public void ApplyForce(Vector2 force)
    {
        if (!this.IsGrabbingStatic())
        {
            rbd.AddForce(force);
        }
    }

    public bool IsGrabbing()
    {
        return this.grabJoint != null;
    }

    public bool IsGrabbingStatic()
    {
        return this.grabJoint != null && this.grabJoint.connectedBody.isKinematic;
    }
}
