using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 10.0f;
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
    public bool blocking = false;
    private Globals glob;
    private bool canCast = true;

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
        if (!canMove) {
            walkanim.Stop();
            return;
        }

        velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        if (Input.IsActionPressed(jumpAction) && IsOnFloor() && canCast){
            //velocity.Y = JumpVelocity;
            canCast = false;
            canMove = false;
            slapanim.SpeedScale = 3;
            HookMove();
        }

        Vector2 inputDir = Input.GetVector(moveLeftAction, moveRightAction, moveUpAction, moveDownAction);
        Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();

        if (direction != Vector3.Zero)
        {
            walkanim.Play("walk"); // should continue from the last state -- currently starts over when new input comes in

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

        GD.Print(@event);
        if (Input.IsActionPressed(blockAction) && canCast)
        {
            canCast = false;
            BlockAttack(true);
        }
        if (Input.IsActionJustReleased(blockAction))
        {
            canCast = true;
            BlockAttack(false);
        }

        if (Input.IsActionPressed(slapAction) && canCast)
        {
            canCast = false;
            slapanim.SpeedScale = 2;
            slapanim.Play("slap");
        }
    }

    public void TakeDmg(int amount, Vector3 enemyPosition)
    {
        if (blockBubble.Visible == true) return;

        hp -= amount;
        GD.Print($"{Name} took dmg: {amount}");
        glob.EmitSignal("RefreshHp", Name, amount);


        Vector3 direction = (GlobalPosition - enemyPosition).Normalized();

        direction.Y = 0;
        direction = direction.Normalized();
        var strength = 7;

        Vector3 targetPosition = GlobalPosition + (direction * strength);

        Tween tween = GetTree().CreateTween();

        tween.TweenProperty(this, "global_position", targetPosition, 0.5)
                .SetTrans(Tween.TransitionType.Quad)
                .SetEase(Tween.EaseType.Out);

    }

    private void BlockAttack(bool state)
    {
        blocking = state;
        blockBubble.Visible = state;
    }

    private void _on_slap_animation_finished(string animname)
    {
        if (animname == "slap")
        {
            slapanim.Play("RESET");
        }
        if (animname == "hook"){
            slapanim.Play("RESET");
            canMove = true;
        }
        canCast = true;
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
                    player.Call("TakeDmg", 1, this.GlobalPosition);
            }
        }
    }


    public void HookMove(){
        // send out a hitbox carry back anyplayers we hit. Wall check?
        GD.Print("wtf");
        slapanim.Play("hook");
    }

    private void _on_armhitarea_body_entered(Node3D area){

        slapanim.Play("RESET");

    }

    private void _on_armhitarea_area_entered(Area3D area){

        if (area.IsInGroup("playerbody")){

            CharacterBody3D player = area.GetParent<CharacterBody3D>();
            if (player == this) return;

            GD.Print(player.Name);
            bool blockingRn = (bool)player.Get("blocking");

            if (blockingRn) return;

            GetTree().CreateTween().TweenProperty(player, "global_position",
             new Vector3(slapbox.GlobalPosition.X, slapbox.GlobalPosition.Y, slapbox.GlobalPosition.Z), 0.5);
        }

    }

}
