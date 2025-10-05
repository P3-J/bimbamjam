using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] PackedScene CharacterSelect;
	private void _on_new_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/selectscreen.tscn");
	}


	[Export] private Button NewButton;
	[Export] private Button SettingsButton;
	[Export] private Button ExitButton;

	public override void _Ready()
	{
		NewButton.Pressed += OnPlayPressed;

		SettingsButton.Pressed += OnOptionsPressed;
		ExitButton.Pressed += OnQuitPressed;

		NewButton.GrabFocus();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (Input.IsActionJustPressed("ui_accept"))
		{
			GetTree().ChangeSceneToFile("res://scenes/selectscreen.tscn");
		}
	}


	private void OnPlayPressed()
	{
		GD.Print("Play pressed");
	}

	private void OnOptionsPressed()
	{
		GD.Print("Options pressed");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			GD.Print("Cancel pressed, maybe show confirm dialog?");
		}
	}


}
