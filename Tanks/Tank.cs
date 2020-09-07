using Godot;
using System;

public class Tank : KinematicBody2D
{
    [Signal]
    public delegate void Shoot();

    [Signal]
    public delegate void HealthChanged();
    [Signal]
    public delegate void Dead();

    [Export]
    PackedScene Bullet;

    [Export] 
    public int MaxSpeed;
    [Export]
    public float RotationSpeed;
    [Export]
    public float GunCoolDown;
    [Export]
    public int Health;

    protected Vector2 velocity = new Vector2();
    protected bool CanShoot = true;
    protected bool Alive = true;




    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var gunTimer = (Timer)GetNode("GunTimer");
        GD.Print($"{gunTimer}");
        gunTimer.WaitTime = GunCoolDown;

        var map = GetNode<Map01>("/root/Map01");
        var error = this.Connect("Shoot", map, "_on_Tank_Shoot");
        
        GD.Print($"{Enum.GetName(typeof(Error), error)}");
        
    }

    public virtual void Control(float delta) 
    {

    }

    protected void shoot()
    {
        if(CanShoot) 
        {
            
            CanShoot = false;
            GetNode<Timer>("GunTimer").Start();
            var turret = GetNode<Node2D>("Turret");
            var dir = new Vector2(1,0).Rotated(turret.GlobalRotation);
            var muzzle = GetNode<Position2D>("Turret/Turret/Muzzle");

            GD.Print($"Shoot {turret.GlobalRotation}");
            EmitSignal("Shoot", Bullet, muzzle.GlobalPosition, dir);
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!Alive) {
            return;
        }

        Control(delta);

        MoveAndSlide(velocity);
    }

    public void _on_GunTimer_timeout()
    {
        CanShoot = true;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
