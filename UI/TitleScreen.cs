using Godot;
using System;

public class TitleScreen : Control
{
    GLOBALS _globals = new GLOBALS();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Input(InputEvent @event)
    {
        GD.Print($"Event: {@event}");
        if(@event.IsActionPressed("ui_select"))
        {
            GLOBALS.CurrentLevel=1;
            //_globals.NextLevel();
            GetTree().ChangeScene(GLOBALS.Levels[GLOBALS.CurrentLevel]);
        }
            
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
