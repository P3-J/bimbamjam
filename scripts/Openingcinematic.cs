using Godot;
using System;

public partial class Openingcinematic : Node3D
{

    public void ChangeNext()
    {
        GetTree().ChangeSceneToFile("res://scenes/selectscreen.tscn");
    }

}
