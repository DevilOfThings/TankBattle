using Godot;
using System;

public class Map01 : Node2D
{
    
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetCameraLimits();

       
        
    }

    
    private void SetCameraLimits()
    {
        TileMap tileMap = (TileMap)GetNode("Ground");
        var mapLimits = tileMap.GetUsedRect();
        var mapCellSize = tileMap.CellSize;
    
    // var xxx = GetNode("Ground/Player");
    
     ((Camera2D)GetNode("Ground/Player/Camera2D")).LimitLeft = (int)(mapLimits.Position.x * mapCellSize.x);
     ((Camera2D)GetNode("Ground/Player/Camera2D")).LimitRight = (int)(mapLimits.End.x * mapCellSize.x);
     ((Camera2D)GetNode("Ground/Player/Camera2D")).LimitTop = (int)(mapLimits.Position.y * mapCellSize.y);
     ((Camera2D)GetNode("Ground/Player/Camera2D")).LimitBottom = (int)(mapLimits.End.y * mapCellSize.y);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    

    public void _on_Tank_Shoot(Node body)
    {
        GD.Print($"_on_Tank_Shoot{body}");
    }

    public void _on_Tank_Shoot(PackedScene bullet, Vector2 position, Vector2 direction) {
    
        GD.Print($"_on_Tank_Shoot {position} {direction}");

        //var bulletScene = ResourceLoader.Load("res://Bullets/PlayerBullet.tscn") as PackedScene;

        //if(bulletScene !=null) {
            var instance = bullet.Instance() as Bullet;
            GD.Print($"{instance.GetType()}");
            AddChild(instance);
            instance.Start(position, direction);
        //}

        
    }
}
