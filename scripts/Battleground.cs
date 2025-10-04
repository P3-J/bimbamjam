using Godot;
using System;

public partial class Battleground : Node3D
{

    public Globals glob;

    [Export] RichTextLabel p1hp;
    [Export] RichTextLabel p2hp;

    [Export] RichTextLabel p1wins;
    [Export] RichTextLabel p2wins;

    [Export] CharacterBody3D p1;
    [Export] CharacterBody3D p2;

    [Export] Camera3D p1cam;
    [Export] Camera3D p2cam;

    [Export] StaticBody3D bg1;
    [Export] StaticBody3D bg2;
    [Export] StaticBody3D bg3;

    [Export] Label fatalitylabel;
    [Export] Timer reloadtimer;
    private bool startedReset = false;


    public override void _Ready()
    {
        base._Ready();
        

        glob = GetNode<Globals>("/root/Globals");
        mapSwitcheroo();

        p1wins.Text = glob.p1wins.ToString() + "W";
        p2wins.Text = glob.p2wins.ToString() + "W";

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
            glob.p2wins += 1;
            fatalitylabel.Visible = true;
            reloadtimer.Start();
        }

        if (p2.GlobalPosition.Y < -25 && !startedReset)
        {
            GD.Print("p1 win");
            startedReset = true;
            p2cam.Current = true;
            glob.p1wins += 1;
            fatalitylabel.Visible = true;
            reloadtimer.Start();
        }

    }

    public void mapSwitcheroo()
    {
        switch (glob.round)
        {
            case 1:
                bg1.GlobalPosition = new Vector3(-11.139f, -3.9f, -36.6f);
                break;
            case 2:
                bg2.GlobalPosition = new Vector3(-11.8f, 3.289f, -39.6f);
                break;
            case 3:
                bg3.GlobalPosition = new Vector3(-12.584f, 4.82f, -39.6f);
                break;
        }
        if (glob.round == 3) glob.round = 0;
    }

    private void _on_timer_timeout()
    {
        glob.round += 1;
        GetTree().ReloadCurrentScene();
    }

    private void refreshBars(string Name)
    {

        if (Name == "player")
        {
            p1hp.Text = (glob.p1Multi * 10).ToString() + "%";
        }
        if (Name == "player2")
        {
            p2hp.Text = (glob.p2Multi * 10).ToString() + "%";
        }

    }


}
