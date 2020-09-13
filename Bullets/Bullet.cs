using Godot;
using System;

public class Bullet : Area2D
{
    [Export]
    public int Speed;
    [Export]
    public int Damage;
    [Export]
    public float Lifetime;
    [Export] float SteerForce;

    Node2D target = null;

    Vector2 velocity = new Vector2();
    Vector2 Acceleration = new Vector2();

    public void Start(Vector2 position, Vector2 direction, Node2D target=null)
    {
        Position = position;
        Rotation = direction.Angle();
        this.target = target;
        var root = this.GetTree().Root;

        GD.Print($"Lifetime: {Lifetime}");


        //GetNode<Timer>("Lifetime").Stop();
        GetNode<Timer>("Lifetime").Start(Lifetime);
        velocity = direction * Speed;
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private Vector2 seek()
    {
        var desired = (target.Position - Position).Normalized() * Speed;
        var steer = (desired-velocity).Normalized() * SteerForce;
        return steer;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    //   if(target!=null)
    //   {
    //       //GD.Print($"Homing: {target}");
    //       Acceleration += seek();
    //       velocity += Acceleration * delta;
    //       velocity = velocity.Clamped(Speed);
    //         Rotation = velocity.Angle();
    //   }
      Position += velocity * delta;

  }

  public void _on_Bullet_body_entered(Node body)
  {
     

        if(body.HasMethod("TakeDamage")) {
            
            var type = body.GetType();
            GD.Print($"{type}");
            var takeDamage = type.GetMethod("TakeDamage");
            GD.Print($"{takeDamage}");
            takeDamage.Invoke(body, new object[] {Damage});
            
        }
        explode();
     
  }

    private void explode()
    {
        GetNode<AnimatedSprite>("Explosion").Show();
        GD.Print($"exploded");
        velocity = new Vector2();
        GetNode<Sprite>("Sprite").Hide();
        
        GetNode<AnimatedSprite>("Explosion").Play("smoke");

    }
  public void _on_Lifetime_timeout()
  {
      GD.Print($"Speed: {Speed} Damage: {Damage} Timeout: {Lifetime}");
      explode();
  }

  public void _on_Explosion_animation_finished()
  {
      QueueFree();
  }
}
