
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
    Rigidbody2D endA;
    Rigidbody2D endB;

    public Transform Center { get; set; }

    void Start()
	{
        cachedRenderer = this.transform.Find("Center").GetComponent<SpriteRenderer>();
        endA = this.transform.Find("End_A").GetComponent<Rigidbody2D>();
        endB = this.transform.Find("End_B").GetComponent<Rigidbody2D>();
        Center = this.transform.Find("Center").GetComponent<Transform>();
        cachedRenderer.color = playerColors[PlayerIndex];
    }


	void Update()
	{
        endB.AddForce(Actions.joystick.Value * forceMultiplier);
	}
}


