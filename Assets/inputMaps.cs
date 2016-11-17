// input masks


// KeyCode enum -> built-in enum for input (keyboard, mouse and up to 8 gamepads supported)

// FPS
public enum InputMapFPS
{
	Forward,		// W
	Backward,		// S
	TurnLeft,		// usually mouse
	TurnRight,		// usually mouse
	StrafeLeft,		// A
	StrafeRight,	// D
	
	Action1,		// E
	Action2,		// Q
	Action3,		// F
	
	Fire,			// usually mouse1
	AltFire,		// usually mouse2
	Reload,			// R
	
	Jump,			// space
	Sprint,			// shift
	Crouch,			// ctrl
	
	Hotbar1,		// 1
	Hotbar2,		// 2
	Hotbar3,		// 3
	Hotbar4,		// 4
	Hotbar5,		// 5
	Hotbar6,		// 6
	
	TabAction,		// tab
	Inventory,		// I
	Map,			// M
}

public enum InputMapPlatformer
{
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3,
}

// final use example:  Input.GetKey (Input1.Jump); //TODO: map input map -> enum -> .GetKey interfaces 

// using FPS as an example
class InputHandler : MonoBehaviour
{
	InputMap actions;
	
	public InputHandler(InputMap actions)
	{
		this.actions = actions;
	}
	
	public void Remap(InputMap actions)
	{
		this.actions = actions;
	}
	
	void Start()
	{
		
	}
	
	void Update()
	{
		// isn't there a way to get a list of all the keys pressed?
		// probably no direct calls should happen here, instead all actions should be raised as an event to the parent
	
		// forward/backwards movement (both together = no movement)
		if(Input.GetKeyDown(actions.Forward) && !Input.GetKeyDown(actions.Backward))
		{
			// go forwards
		}
		else if(Input.GetKeyDown(actions.Backward) && !Input.GetKeyDown(actions.Forward))
		{
			// go backwards
		}
		
		// left/right movement (both together = no movement)
		if(Input.GetKeyDown(actions.StrafeLeft) && !Input.GetKeyDown(actions.StrafeRight))
		{
			// strafe left
		}
		else if(Input.GetKeyDown(actions.StrafeRight) && !Input.GetKeyDown(actions.StrafeLeft))
		{
			// strafe right
		}
		
		if(Input.GetKeyDown(actions.Hotbar1))
		{
			// equip item in hotbar slot 1
		}
		if(Input.GetKeyDown(actions.Hotbar2))
		{
			// equip item in hotbar slot 2
		}
		if(Input.GetKeyDown(actions.Hotbar3))
		{
			// equip item in hotbar slot 3
		}
		if(Input.GetKeyDown(actions.Hotbar4))
		{
			// equip item in hotbar slot 4
		}
		if(Input.GetKeyDown(actions.Hotbar5))
		{
			// equip item in hotbar slot 5
		}
		if(Input.GetKeyDown(actions.Hotbar6))
		{
			// equip item in hotbar slot 6
		}
		
	}
}

class InputMap
{
	
}
