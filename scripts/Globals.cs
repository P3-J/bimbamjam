using Godot;
using System;

public partial class Globals : Node
{

    [Signal] public delegate void RefreshHpEventHandler(string playerName, int dmgAmount);

    public string p1pick = "";
    public string p2pick = "";

}
