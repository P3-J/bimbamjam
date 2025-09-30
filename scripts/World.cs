using Godot;
using System;

public partial class World : Node2D
{

    [Export] RichTextLabel textbox;

    public override void _Ready()
    {
        base._Ready();


    }

    private void _on_button_pressed()
    {
        textbox.Text = "Retard";

        Tween twen = GetTree().CreateTween();
        GetNode<Button>("Button").Visible = false;
        twen.TweenProperty(textbox, "scale", new Vector2(5, 5), 1);
        twen.TweenProperty(textbox, "rotation", 360, 50);

    }
}
