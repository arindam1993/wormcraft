using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnd : MonoBehaviour
{
    public Rigidbody2D grabbableObject;
    public float grabRadius;

    //This joint is dynmically created in order to grab things
    private FixedJoint2D grabJoint;

    private int layerMask;
    private Rigidbody2D rbd;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.NameToLayer("Grabbable");
        rbd = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(this.transform.position, grabRadius, 1 << layerMask);
        Collider2D overlap = null;
        //Find first object that is not myself
        foreach( Collider2D overlapped in overlaps)
        {
            if(overlapped.gameObject != this.gameObject)
            {
                overlap = overlapped;
                break;
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
}
