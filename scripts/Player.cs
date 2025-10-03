using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 8.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float RotationSpeed = 8.0f;
    [Export] public Area3D slapbox;
    [Export] public MeshInstance3D blockBubble;
    [Export] public AnimationPlayer walkanim;
    [Export] public AnimationPlayer slapanim;
    [Export] public bool canMove = true;

    [Export] public bool isPlayerOne = true; // true = WASD, false = arrows

    private string moveLeftAction;
    private string moveRightAction;
    private string moveUpAction;
    private string moveDownAction;
    private string jumpAction;
    private string blockAction;
    private string slapAction;

    private int hp = 10;
    private bool blocking = false;
    private Globals glob;

    private float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    private Vector3 velocity;

    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");

        // Assign controls based on player number
        if (isPlayerOne)
        {
            moveLeftAction = "p1_left";
            moveRightAction = "p1_right";
            moveUpAction = "p1_up";
            moveDownAction = "p1_down";
            jumpAction = "p1_jump";
            blockAction = "p1_block";
            slapAction = "p1_slap";
        }
        else
        {
            moveLeftAction = "p2_left";
            moveRightAction = "p2_right";
            moveUpAction = "p2_up";
            moveDownAction = "p2_down";
            jumpAction = "p2_jump";
            blockAction = "p2_block";
            slapAction = "p2_slap";
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!canMove) return;

        velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        if (Input.IsActionJustPressed(jumpAction) && IsOnFloor())
            velocity.Y = JumpVelocity;

        Vector2 inputDir = Input.GetVector(moveLeftAction, moveRightAction, moveUpAction, moveDownAction);
        Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();

        if (direction != Vector3.Zero)
        {
            walkanim.Play("walk");

            float targetAngle = Mathf.Atan2(direction.X, direction.Z);
            Rotation = new Vector3(
                Rotation.X,
                Mathf.LerpAngle(Rotation.Y, targetAngle, (float)(delta * RotationSpeed)),
                Rotation.Z
            );

            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            walkanim.Stop();
            velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionJustPressed(blockAction))
            BlockAttack(true);

        if (Input.IsActionJustReleased(blockAction))
            BlockAttack(false);

        if (Input.IsActionJustReleased(slapAction))
        {
            slapanim.SpeedScale = 2;
            slapanim.Play("slap");
        }
    }

    public void TakeDmg(int amount)
    {
        hp -= amount;
        GD.Print($"{Name} took dmg: {amount}");
        glob.EmitSignal("RefreshHp", Name, amount);
    }

    private void BlockAttack(bool state)
    {
        blocking = state;
        // blockBubble.Visible = state;
    }

    private void _on_slap_animation_finished(string animname)
    {
        if (animname == "slap")
        {
            // slapbox.Monitoring = false;
            return;
        }
    }

    public void enableSlapBox()
    {
        foreach (Area3D area in slapbox.GetOverlappingAreas())
        {
            GD.Print("Overlapping area: " + area.Name);
            if (area.IsInGroup("playerbody"))
            {
                CharacterBody3D player = area.GetParent<CharacterBody3D>();
                if (player != this) 
                    player.Call("TakeDmg", 1);
            }
        }
    }
}
