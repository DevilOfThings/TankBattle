using Godot;
using System;

public class Tank : KinematicBody2D
{
    [Signal]
    public delegate void Shoot();

    [Signal]
    public delegate void HealthChanged();
    [Signal]
    public delegate void AmmoChanged();
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
    protected int health;

    [Export]
    public int MaxAmmo =20;
    [Export]
    public int Ammo
    {
        get => ammo;
        set{
            
            ammo = Math.Min(value, MaxAmmo);
            EmitSignal("AmmoChanged", ammo*100/MaxAmmo);
        }
    }

    protected int ammo =-1;

    [Export]
    public int GunShots = 1;
    [Export]
    public float GunSpread = 0.2f;
    [Export]
    public float OffRoadFriction =0.5f;

    protected Vector2 velocity = new Vector2();
    protected bool CanShoot = true;
    protected bool CanShoot2 = true;
    protected bool Alive = true;

    public TileMap Map;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Particles2D>("Smoke").Emitting = false;
        health = MaxHealth;
        var gunTimer = (Timer)GetNode("GunTimer");
        GD.Print($"{gunTimer}");
        gunTimer.WaitTime = GunCoolDown;

        // var found = GetTree();
        // GD.Print($"{found.GetType()} ");
        // var root = found.Root;
        // foreach(Node child in root.GetChildren())
        // {
        //     GD.Print($"{child.GetParent().GetType()} {child.GetParent()} {child.GetType()} {child.Name}");
        // }
        // GD.Print($"{root.GetType()} {root.Name}");
        var map = GetNode("/root/Map01");
        GD.Print($"Map: {map}");
        if(map !=null)
        {
            
            GD.Print($"{map.GetType()} {map.Name}");
            var error = this.Connect("Shoot", map, "_on_Tank_Shoot");
            
            EmitSignal("HealthChanged", health*100/MaxHealth);
            EmitSignal("AmmoChanged", ammo*100/MaxAmmo);
            
            GD.Print($"{Enum.GetName(typeof(Error), error)}");
        }
         
        
    }

    public virtual void Control(float delta) 
    {

    }

    protected void shoot(int shots, float spread, Node2D target = null)
    {
        if(CanShoot && Ammo !=0) 
        {
            Ammo -=1;
            GD.Print($"Can shoot");
            CanShoot = false;
            GetNode<Timer>("GunTimer").Start();
            var turret = GetNode<Node2D>("Turret");
            var dir = new Vector2(1,0).Rotated(turret.GlobalRotation);
            var muzzle = GetNode<Position2D>("Turret/Turret/Muzzle");
            GetNode<AnimationPlayer>("AnimationPlayer").Play("muzzle_flash");


            if(shots>1)
            {
                for(var i =1; i <= shots; ++i )
                {
                    var a = -spread + i *(2*spread)/(shots-1);
                    EmitSignal("Shoot", Bullet, muzzle.GlobalPosition, dir.Rotated(a), target);
                }
            }
            else {
                GD.Print($"Shoot {turret.GlobalRotation}");
                EmitSignal("Shoot", Bullet, muzzle.GlobalPosition, dir, target);
            }
            
        }
    }

    protected void shoot2(Node2D target = null)
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

                //GD.Print($"Shoot {turret.GlobalRotation}");
                EmitSignal("Shoot", Bullet, muzzle.GlobalPosition, dir, target);
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!Alive) {
            return;
        }

        Control(delta);

        
        

         if(Map!=null) {
             var tile = Map.GetCellv(Map.WorldToMap(Position));
            
            var slow =Array.Find(GLOBALS.SlowTerrain, (x) => x == tile );
             if(0 !=slow)
             {
                 velocity *= OffRoadFriction;
             }
             //GD.Print($"{velocity} {slow}");
         }
        
        MoveAndSlide(velocity);
    }

    public void TakeDamage(int amount)
    {
        health-=amount;
        EmitSignal("HealthChanged", health * 100/MaxHealth);

        if(health <= 0) {
            Explode();
        }

        GetNode<Particles2D>("Smoke").Emitting = (health < MaxHealth/2) ? true : false; 

        if(this is Player && health <=0)
        {
            EmitSignal("Dead");
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
