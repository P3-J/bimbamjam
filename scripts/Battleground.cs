using Godot;
using System;

public partial class Battleground : Node3D
{

    public Globals glob;

    [Export] ProgressBar p1hpbar;
    [Export] ProgressBar p2hpbar;


    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");

        glob.Connect("RefreshHp", new Callable(this, nameof(refreshBars)));

    }


    private void refreshBars(string Name, int amount)
    {

        GD.Print(Name, "-",amount);

        if (Name == "player")
        {
            p1hpbar.Value -= amount;
        }
        if (Name == "player2")
        {
            p2hpbar.Value -= amount;
        }

    }


}
