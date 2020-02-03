
using InControl;
using UnityEngine;
using System.Collections.Generic;


// This is just a simple "player" script that rotates and colors a cube
// based on input read from the device on its inputDevice field.
//
// See comments in PlayerManager.cs for more details.
//
public class Player : BaseDisableable
{
    public PlayerActions Actions { get; set; }
    public int PlayerIndex { get; set; }

    Color[] playerColors = { Color.green, Color.red, Color.blue, Color.yellow };
    public float forceMultiplier;
    
    List<Vector2> initialRelativePositions = new List<Vector2>();
    List<GameObject> childGameObjects = new List<GameObject>();
    WormEnd endA;
    WormEnd endB;

    public Bounds displayBounds { get; set; }
    SpriteRenderer outlineA;
    SpriteRenderer outlineB;

    void Start()
	{
        int count = this.transform.childCount;
        // track initial setup so we can reset state when enabling and siabling the worm
        for (int i=0; i < count; i++)
        {
            Transform child = this.transform.GetChild(i);

            initialRelativePositions.Add(child.position - this.transform.position);
            childGameObjects.Add(child.gameObject);
        }
        Color playerColor = playerColors[PlayerIndex];
        endA = this.transform.Find("End_A").GetComponent<WormEnd>();
        outlineA = endA.transform.Find("Outline").GetComponent<SpriteRenderer>();
        outlineA.color = playerColor;

        endB = this.transform.Find("End_B").GetComponent<WormEnd>();
        outlineB = endB.transform.Find("Outline").GetComponent<SpriteRenderer>();
        outlineB.color = playerColor;

        displayBounds = CalcDisplayBounds();
    }


	void Update()
	{
        if (!IsWormcraftDisabled())
        {
            if (Actions.GrabA.IsPressed)
            {
                endA.Release();
            }
            else
            {
                endA.Grab();
            }

            if (Actions.GrabB.IsPressed)
            {
                endB.Release();
            }
            else
            {
                endB.Grab();
            }
            WormEnd grabEnd = GetFreeEnd();
            if (grabEnd != null)
            {
                grabEnd.ApplyForce(Actions.joystick.Value * forceMultiplier);
            }
        }
       
        displayBounds = CalcDisplayBounds();
    }


    Bounds CalcDisplayBounds()
    {
        // Encapsulate both our end parts into our single display bounds
        Bounds bounds = new Bounds(endA.displayBounds.min, new Vector3(0, 0, 0));
        bounds.Encapsulate(endA.displayBounds.max);
        bounds.Encapsulate(endB.displayBounds.min);
        bounds.Encapsulate(endB.displayBounds.max);

        return bounds;
    }


    // returns a worm end only when one end is free
    WormEnd GetFreeEnd()
    {   
        //No end is free
        if(endA.IsGrabbingStatic() && endB.IsGrabbingStatic())
        {
            return null;
        }

        return endA.IsGrabbingStatic() ? endB : endA;
    }

    public override void WormcraftDisable()
    {
        base.WormcraftDisable();

        for(int cIdx = 0; cIdx < childGameObjects.Count; cIdx ++) 
        {
            GameObject child = childGameObjects[cIdx];
            child.GetComponent<Rigidbody2D>().simulated = false;
            child.GetComponent<SpriteRenderer>().enabled = false;
        }
        for (int cIdx = 0; cIdx < childGameObjects.Count; cIdx++)
        {
            GameObject child = childGameObjects[cIdx];
            child.transform.position = new Vector2(this.transform.position.x, this.transform.position.y) + initialRelativePositions[cIdx];
        }
        outlineA.enabled = false;
        outlineB.enabled = false;

    }

    public override void WormcraftEnable()
    {
        base.WormcraftEnable();
        for (int cIdx = 0; cIdx < childGameObjects.Count; cIdx++)
        {
            GameObject child = childGameObjects[cIdx];
            child.GetComponent<Rigidbody2D>().simulated = true;
            child.GetComponent<SpriteRenderer>().enabled = true;
            child.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            child.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        }
        outlineA.enabled = true;
        outlineB.enabled = true;

    }

    public override Vector2 GetCenter()
    {
        return childGameObjects[childGameObjects.Count / 2].transform.position;
    }
}


