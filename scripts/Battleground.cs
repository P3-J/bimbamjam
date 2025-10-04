using Godot;
using System;

public partial class Battleground : Node3D
{

    public Globals glob;

    [Export] ProgressBar p1hpbar;
    [Export] ProgressBar p2hpbar;

    [Export] CharacterBody3D p1;
    [Export] CharacterBody3D p2;

    [Export] Camera3D p1cam;
    [Export] Camera3D p2cam;

    [Export] Label fatalitylabel;
    [Export] Timer reloadtimer;
    private bool startedReset = false;


    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");

        glob.Connect("RefreshHp", new Callable(this, nameof(refreshBars)));

    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (p1.GlobalPosition.Y < -25 && !startedReset)
        {
            GD.Print("p2 win");
            p1cam.Current = true;
            startedReset = true;
            fatalitylabel.Visible = true;
            reloadtimer.Start();
        }

        if (p2.GlobalPosition.Y < -25 && !startedReset)
        {
            GD.Print("p1 win");
            startedReset = true;
            p2cam.Current = true;
            fatalitylabel.Visible = true;
            reloadtimer.Start();
        }

    }

    private void _on_timer_timeout()
    {
        GetTree().ReloadCurrentScene();
    }

    private void refreshBars(string Name, int amount)
    {

        GD.Print(Name, "-", amount);

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
