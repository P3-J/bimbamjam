using Godot;
using System;

public partial class testProgressBar : Node
{
	[Export] PackedScene testProgress;
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			Control gaytest = testProgress.Instantiate<Control>();
			AddChild(gaytest);
		}
	}

}
