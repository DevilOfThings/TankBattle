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

    Vector2 velocity = new Vector2();

    public void Start(Vector2 position, Vector2 direction)
    {
        Position = position;
        Rotation = direction.Angle();
        
        var root = this.GetTree().Root;

        GD.Print($"{root}");

        GetNode<Timer>("Lifetime").WaitTime = Lifetime;
        velocity = direction * Speed;
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      Position += velocity * delta;
  }

  public void _on_Bullet_body_entered(Node body)
  {
      if(body.HasMethod("TakeDamage")) {
          // Invoke???
          
      }
      explode();
  }

    private void explode()
    {
        QueueFree();
    }
  public void _on_Lifetime_timeout()
  {
      explode();
  }
}
