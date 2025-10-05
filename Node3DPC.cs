using Godot;
using System;

public partial class Node3DPC : Node3D
{
	[Export] AnimationPlayer animplayer;
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (Input.IsActionJustPressed("ui_accept"))
		{
			animplayer.Play("move");
		}
	}

	public void switchScene()
	{
		GetTree().ChangeSceneToFile("res://scenes/openingcinematic.tscn");
	}

	private void _on_closing_animation_finished(string animname)
	{
		switchScene();
	}
}
