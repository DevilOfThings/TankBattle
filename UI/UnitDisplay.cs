using Godot;
using System;

public class UnitDisplay : Node2D
{
    Texture redBar;
    Texture yellowBar;
    Texture greenBar;
    Texture barTexture;

    Tween HealthBarTween;

    public void UpdateHealthBar(int value)
    {
        GD.Print($"{GetTree().Root}");
       

        barTexture = greenBar; 

        if(value < 60) {
            barTexture = yellowBar;
        }
        if(value < 25) {
            barTexture = redBar;
        }
        
        if(value < 100) {
            GetNode<TextureProgress>("HealthBar").Show();
        }
        GetNode<TextureProgress>("HealthBar").Value = value;
        
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print($"Tree: {GetTree().Root.Name}");
         foreach(Node child in GetChildren()) {
             
             GD.Print($"Child: {child} {child.Name}");
             if(child is CanvasItem)
                ((CanvasItem)child).Hide();
         }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      
      GlobalRotation = 0;
  }
}
