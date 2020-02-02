
using InControl;


public class PlayerActions : PlayerActionSet
{
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;
	public PlayerAction Join;
    public PlayerAction GrabA;
    public PlayerAction GrabB;
    public PlayerTwoAxisAction joystick;


	public PlayerActions()
	{
		Left = CreatePlayerAction( "Left" );
		Right = CreatePlayerAction( "Right" );
		Up = CreatePlayerAction( "Up" );
		Down = CreatePlayerAction( "Down" );
		Join = CreatePlayerAction("Join");

        GrabA = CreatePlayerAction("GrabA");
        GrabB = CreatePlayerAction("GrabB");
		joystick = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
	}


	public static PlayerActions CreateWithKeyboardBindings()
	{
		var actions = new PlayerActions();

		actions.Join.AddDefaultBinding( Key.Space );
        actions.GrabA.AddDefaultBinding(Key.Z);
        actions.GrabB.AddDefaultBinding(Key.X);

        actions.Up.AddDefaultBinding( Key.UpArrow );
		actions.Down.AddDefaultBinding( Key.DownArrow );
		actions.Left.AddDefaultBinding( Key.LeftArrow );
		actions.Right.AddDefaultBinding( Key.RightArrow );

		return actions;
	}


	public static PlayerActions CreateWithJoystickBindings()
	{
		var actions = new PlayerActions();

		actions.Join.AddDefaultBinding( InputControlType.Command );
        actions.GrabA.AddDefaultBinding( InputControlType.Button1 );
        actions.GrabB.AddDefaultBinding(InputControlType.Button2);

        actions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
		actions.Down.AddDefaultBinding( InputControlType.LeftStickDown );
		actions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
		actions.Right.AddDefaultBinding( InputControlType.LeftStickRight );

		return actions;
	}
}

