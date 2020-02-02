using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnd : MonoBehaviour
{
    private Rigidbody2D grabbableObject;
    public float grabRadius;

    //This joint is dynmically created in order to grab things
    private FixedJoint2D grabJoint;

    private int layerMask;
    private Rigidbody2D rbd;
    private SpriteRenderer rend;
    private GameObject otherEnd;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.NameToLayer("Grabbable");
        rbd = this.GetComponent<Rigidbody2D>();
        rend = this.GetComponent<SpriteRenderer>();
        string otherName = this.gameObject.name == "End_A" ? "End_B" : "End_B";
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
    }

    public void Grab()
    {
        if(grabbableObject != null && this.grabJoint == null)
        {
            //Grab by creating a fixed joint between this end and the grabbable object
            grabJoint = this.gameObject.AddComponent<FixedJoint2D>();
            grabJoint.connectedBody = grabbableObject;


            //TODO: sound
        }
    }

    public void Release()
    {
        if(this.grabJoint != null)
        {
            this.grabJoint.connectedBody = null;
            Component.Destroy(this.grabJoint);
            this.grabJoint = null;

            //TODO: sound
        }
    }

    public void ApplyForce(Vector2 force)
    {
        if (!this.IsGrabbing())
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
