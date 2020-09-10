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

        GD.Print($"Lifetime: {Lifetime}");


        //GetNode<Timer>("Lifetime").Stop();
        GetNode<Timer>("Lifetime").Start(Lifetime);
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
