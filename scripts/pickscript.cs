using Godot;
using GodotPlugins.Game;
using System;

public partial class pickscript : Node3D
{

    [Export] Label mainlabel;
    [Export] Button ulemistebutton;
    [Export] Button frogbutton;
    [Export] Button Startbutton;

    [Export] SpotLight3D light1;
    [Export] SpotLight3D light2;

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

    private void _on_button_2_mouse_entered()
    {
        light2.LightEnergy = 3;
    }

    private void _on_button_2_mouse_exited()
    {
        light2.LightEnergy = 0;
    }

    private void _on_button_3_mouse_entered()
    {
        light1.LightEnergy = 3;
    }

    private void _on_button_3_mouse_exited()
    {
        light1.LightEnergy = 0;
    }

    private void _on_button_pressed()
    {
        glob.start_bg_music();
        GetTree().ChangeSceneToFile("res://scenes/battleground.tscn");
    }

}
