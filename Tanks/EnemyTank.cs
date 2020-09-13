using Godot;
using System;


public class EnemyTank : Tank
{

    [Export]
    public int TurretSpeed;
    [Export]
    public int DetectRadius;

    private float speed=0;
    
    

    private Node2D target;

    public static float Lerp(float value1, float value2, float amount)
    {
        return value1 + (value2 - value1) * amount;
    }

    public override void Control(float delta)
    {
        var parent = GetParent() as PathFollow2D;

        if(parent != null) {

            var lookAheadL = GetNode<RayCast2D>("LookAheadL");
            var lookAheadR = GetNode<RayCast2D>("LookAheadR");

            if(lookAheadL.IsColliding() || lookAheadR.IsColliding()) {
                
                speed = Lerp(speed, 0, 0.3f);
            }
            else {
                speed = Lerp(speed, MaxSpeed, 0.3f);
            }

            parent.Offset = (parent.Offset + speed * delta);
            Position= new Vector2(0,0);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        var circle = new CircleShape2D();

        ((CollisionShape2D)GetNode("DetectRadius/CollisionShape2D")).Shape = circle;
        ((CircleShape2D)((CollisionShape2D)GetNode("DetectRadius/CollisionShape2D")).Shape).Radius = DetectRadius;        
    }

    public void _on_DetectRadius_body_entered(Node body)
    {
        if(body.Name == "Player") {
            //GD.Print($"{body.GetType()}");
            target = (Node2D)body;
        }
    }

    public void _on_DetectRadius_body_exited(Node body)
    {
        if(body ==target) {
            target = null;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      var parent = GetParent() as PathFollow2D;

      if(target !=null) {
          var targetDir = (target.GlobalPosition - GlobalPosition).Normalized();
          var currentDir = new Vector2(1,0).Rotated(((Node2D)GetNode("Turret")).GlobalRotation);
          var leftAngle = currentDir.LinearInterpolate(targetDir, TurretSpeed*delta).Angle();

          if(parent != null)
          {
            ((Node2D)GetNode("Turret")).GlobalRotation = leftAngle;
          }
          else if(leftAngle <= 1.1 && leftAngle >= -1.1) {
            ((Node2D)GetNode("Turret")).GlobalRotation = leftAngle;
          }
            


        if(targetDir.Dot(currentDir) >0.8) {
           shoot(GunShots, GunSpread, target);
        }
        
        
        if(parent == null) 
        {
            var targetDir2 = (GlobalPosition - target.GlobalPosition).Normalized();
            var currentDir2 = new Vector2(1,0).Rotated(((Node2D)GetNode("Turret2")).GlobalRotation);
            var rightAngle = currentDir2.LinearInterpolate(targetDir2, TurretSpeed*delta).Angle();
            if(rightAngle <= 1.1 && rightAngle >= -1.1)
                ((Node2D)GetNode("Turret2")).GlobalRotation = rightAngle;
            

            if(targetDir2.Dot(currentDir2) >0.8) {
                shoot2();
            }
            
            //GD.Print($"LeftAngle: {leftAngle} RightAngle: {rightAngle}");
        }
      }
  }
}
