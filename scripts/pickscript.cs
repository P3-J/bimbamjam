using Godot;
using GodotPlugins.Game;
using System;

public partial class pickscript : Node3D
{

    [Export] Label mainlabel;
    [Export] Button ulemistebutton;
    [Export] Button frogbutton;
    [Export] Button Startbutton;

    public string phase = "p1";

    Globals glob;

    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");

        mainlabel.Text = "P1 \n Pick a fighter";
        Startbutton.Visible = false;

    }

    private void _on_button_2_pressed()
    {
        if (phase == "p1")
        {
            glob.p1pick = "ulemiste";
        }
        if (phase == "p2")
        {
            glob.p2pick = "ulemiste";
        }
        nextPick();
    }

    private void _on_button_3_pressed()
    {
        if (phase == "p1")
        {
            glob.p1pick = "frog";
        }
        if (phase == "p2")
        {
            glob.p2pick = "frog";
        }
        nextPick();
    }

    private void nextPick()
    {
        if (phase == "p2")
        {
            phase = "start";
            Startbutton.Visible = true;
            ulemistebutton.Visible = false;
            frogbutton.Visible = false;
            mainlabel.Visible = false;
        }
        if (phase == "p1")
        {
            phase = "p2";
            mainlabel.Text = "P2 \n Pick a fighter";
        }
        



    }

    private void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/battleground.tscn");
    }

}
