using Godot;
using System;

public partial class Node3DPC : Node3D
{
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (Input.IsActionJustPressed("ui_accept"))
		{
			GetTree().ChangeSceneToFile("res://scenes/selectscreen.tscn");
		}
	}
}
