using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnd : MonoBehaviour
{
    private Rigidbody2D grabbableObject;

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
        Collider2D overlap = Physics2D.OverlapCircle(this.transform.position, 3.0f, layerMask);
        if(overlap != null)
        {
            grabbableObject = overlap.gameObject.GetComponent<Rigidbody2D>();
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
