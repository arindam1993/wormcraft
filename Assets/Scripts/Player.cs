
using InControl;
using UnityEngine;
using System.Collections.Generic;


// This is just a simple "player" script that rotates and colors a cube
// based on input read from the device on its inputDevice field.
//
// See comments in PlayerManager.cs for more details.
//
public class Player : MonoBehaviour
{
    public PlayerActions Actions { get; set; }
    public int PlayerIndex { get; set; }

    Color[] playerColors = { Color.green, Color.red, Color.blue, Color.yellow };
    public float forceMultiplier;

    List<SpriteRenderer> cachedRenderers = new List<SpriteRenderer>();
    WormEnd endA;
    WormEnd endB;

    public Bounds displayBounds { get; set; }

    void Start()
	{
        int count = this.transform.childCount;
        for (int i=0; i < count; i++)
        {
            Transform child = this.transform.GetChild(i);
            if (child.gameObject.name.Contains("Seg"))
            {
                SpriteRenderer currentRenderer = child.gameObject.GetComponent<SpriteRenderer>();
                cachedRenderers.Add(currentRenderer);
                //Color newColor = playerColors[PlayerIndex] * 0.3f;
                //newColor.a = 1.0f;
                //currentRenderer.color = newColor;
            }
        }
        Color playerColor = playerColors[PlayerIndex];
        endA = this.transform.Find("End_A").GetComponent<WormEnd>();
        endA.transform.Find("Outline").GetComponent<SpriteRenderer>().color = playerColor;

        endB = this.transform.Find("End_B").GetComponent<WormEnd>();
        endB.transform.Find("Outline").GetComponent<SpriteRenderer>().color = playerColor;

        displayBounds = CalcDisplayBounds();
    }


	void Update()
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
        if(grabEnd != null)
        {
            grabEnd.ApplyForce(Actions.joystick.Value * forceMultiplier);
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
}


