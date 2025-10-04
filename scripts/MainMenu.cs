using Godot;
using System;

[Export] PackedScene CharacterSelect
public partial class MainMenu : Control
{
    private void _on_new_button_pressed()
    {
        GetTree().ChangeScene("res://")
    }
}
