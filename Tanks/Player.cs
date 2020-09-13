using Godot;
using System;

public class Player : Tank
{
    
    public override void Control(float delta)
    {
        ((Node2D)GetNode("Turret")).LookAt(GetGlobalMousePosition());

        var rotateDirection =0;
        if(Input.IsActionPressed("turn_right")) {
            rotateDirection +=1;
        }
        if(Input.IsActionPressed("turn_left")) {
            rotateDirection -=1;
        }

        Rotation += RotationSpeed * rotateDirection * delta;
        velocity = new Vector2();
        if(Input.IsActionPressed("forward")) {
            velocity = new Vector2(MaxSpeed, 0).Rotated(Rotation);
        }
        if(Input.IsActionPressed("back")) {
            velocity = new Vector2(-MaxSpeed/2, 0).Rotated(Rotation);
        }
        if(Input.IsActionPressed("click")) {
            shoot(1,1);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    public void Heal(int amount)
    {
        health += amount;
        
        health = Math.Min(health, MaxHealth);
        EmitSignal("HealthChanged", health * 100/MaxHealth);        
    }

    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
