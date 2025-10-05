using Godot;
using System;

public partial class Globals : Node
{

    [Signal] public delegate void RefreshHpEventHandler(string playerName);
    [Export] AudioStreamPlayer bgmusic;

    public string p1pick = "";
    public string p2pick = "";

    public float p1Multi = 0;
    public float p2Multi = 0;

    public int p1wins = 0;
    public int p2wins = 0;

    public int round = 1;


    private void _on_bgmusic_finished()
    {
        bgmusic.Play();
    }

    public void start_bg_music()
    {
        bgmusic.Play();
    }

}

