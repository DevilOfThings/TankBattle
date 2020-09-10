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
    public int MaxHealth;
    private int health;

    protected Vector2 velocity = new Vector2();
    protected bool CanShoot = true;
    protected bool CanShoot2 = true;
    protected bool Alive = true;




    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        health = MaxHealth;
        var gunTimer = (Timer)GetNode("GunTimer");
        GD.Print($"{gunTimer}");
        gunTimer.WaitTime = GunCoolDown;

        var map = GetNode<Map01>("/root/Map01");
        var error = this.Connect("Shoot", map, "_on_Tank_Shoot");
        
        EmitSignal("HealthChanged", health*100/MaxHealth);
        
        GD.Print($"{Enum.GetName(typeof(Error), error)}");
        
    }

    public virtual void Control(float delta) 
    {

    }

    protected void shoot()
    {
        if(CanShoot) 
        {
            GD.Print($"Can shoot");
            CanShoot = false;
            GetNode<Timer>("GunTimer").Start();
            var turret = GetNode<Node2D>("Turret");
            var dir = new Vector2(1,0).Rotated(turret.GlobalRotation);
            var muzzle = GetNode<Position2D>("Turret/Turret/Muzzle");
            GetNode<AnimationPlayer>("AnimationPlayer").Play("muzzle_flash");

            GD.Print($"Shoot {turret.GlobalRotation}");
            EmitSignal("Shoot", Bullet, muzzle.GlobalPosition, dir);
        }
    }

    protected void shoot2()
    {
        if(CanShoot2)         
        {
            
            if(FindNode("Turret2") != null)
            {
                CanShoot2 = false;
                GetNode<Timer>("GunTimer").Start();
                var turret = GetNode<Node2D>("Turret2/Turret/Muzzle");
                var dir = new Vector2(1,0).Rotated(turret.GlobalRotation);
                var muzzle = GetNode<Position2D>("Turret2/Turret/Muzzle");
                GetNode<AnimationPlayer>("AnimationPlayer").Play("muzzle_flash2");

                GD.Print($"Shoot {turret.GlobalRotation}");
                EmitSignal("Shoot", Bullet, muzzle.GlobalPosition, dir);
            }
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

    public void TakeDamage(int amount)
    {
        health-=amount;
        EmitSignal("HealthChanged", health * 100/MaxHealth);

        if(health <= 0) {
            Explode();
        }
    }

    public void Explode()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        Alive = false;
        GetNode<Node2D>("Turret").Hide();
        GetNode<Sprite>("Body").Hide();
        GetNode<AnimatedSprite>("Explosion").Show();
        GetNode<AnimatedSprite>("Explosion").Play("fire");

    }
    public void _on_GunTimer_timeout()
    {
        CanShoot = true;
        CanShoot2 = true; // not correct
    }

    public void _on_Explosion_animation_finished()
    {
        QueueFree();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
