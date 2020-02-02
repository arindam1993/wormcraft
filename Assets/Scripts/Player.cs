
using InControl;
using UnityEngine;


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

    SpriteRenderer cachedRenderer;
    WormEnd endA;
    WormEnd endB;


	void Start()
	{
        cachedRenderer = this.transform.Find("Center").GetComponent<SpriteRenderer>();
        endA = this.transform.Find("End_A").GetComponent<WormEnd>();
        endB = this.transform.Find("End_B").GetComponent<WormEnd>();
        cachedRenderer.color = playerColors[PlayerIndex];
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
        
    }
    
    // returns a worm end only when one end is free
    WormEnd GetFreeEnd()
    {   
        //No end is free
        if(endA.IsGrabbing() && endB.IsGrabbing())
        {
            return null;
        }

        if(!endA.IsGrabbing() && !endB.IsGrabbing())
        {
            return null;
        }

        return !endA.IsGrabbing() ? endA : endB;
    } 

}


