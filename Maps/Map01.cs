using Godot;
using System;

public class Map01 : Node2D
{
    
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetCameraLimits();
        Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/UI/crossair_white.png"), Input.CursorShape.Arrow, new Vector2(16,16));
       
        var parent = GetParent();
        
        var tileMap = GetNode<TileMap>("Ground");
        var player = GetNode<Player>("Ground/Player");        
        GD.Print($"{tileMap.Name} {player.Name}");
        player.Map = tileMap;
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
    

   
    public void _on_Tank_Shoot(PackedScene bullet, Vector2 position, Vector2 direction, Node2D tank) {
    
        GD.Print($"_on_Tank_Shoot {position} {direction}");
       
        var instance = bullet.Instance() as Bullet;
        GD.Print($"{instance.GetType()}");
        AddChild(instance);
        instance.Start(position, direction, tank);

    }

    public void _on_Player_Dead()
    {
        GD.Print("Player Dead!!!!");
        GetTree().ChangeScene(GLOBALS.Levels[0]);
    }
}
