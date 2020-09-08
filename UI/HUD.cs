using Godot;
using System;

public class HUD : CanvasLayer
{

    Texture redBar;
    Texture yellowBar;
    Texture greenBar;
    Texture barTexture;

    Tween HealthBarTween;

    public void UpdateHealthBar()
    {

    }
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

        if(HealthBarTween !=null) {

        
        GetNode<TextureProgress>("Margin/Container/HealthBar").TextureProgress_ = barTexture;
        HealthBarTween.InterpolateProperty(
             GetNode<TextureProgress>("Margin/Container/HealthBar"), 
             "value", 
             GetNode<TextureProgress>("Margin/Container/HealthBar").Value,
             value,
             0.2f,
             Tween.TransitionType.Linear,
             Tween.EaseType.InOut
         );
         GetNode<Tween>("Margin/Container/HealthBar/Tween").Start();
         GetNode<AnimationPlayer>("AnimationPlayer").Play("healthBarFlash");
        }
        //GetNode<TextureProgress>("Margin/Container/HealthBar").Value = value;


    }
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        redBar = ResourceLoader.Load("res://Assets/UI/barHorizontal_red_mid 200.png") as Texture;
        yellowBar = ResourceLoader.Load("res://Assets/UI/barHorizontal_yellow_mid 200.png") as Texture;
        greenBar = ResourceLoader.Load("res://Assets/UI/barHorizontal_green_mid 200.png") as Texture;

        HealthBarTween = GetNode<Tween>("Margin/Container/HealthBar/Tween");
        UpdateHealthBar(100);

        
    }

    public void _on_AnimationPlayer_animation_finished(string name)
    {
        if(name == "healthBarFlash") {
            GetNode<TextureProgress>("Margin/Container/HealthBar").TextureProgress_ = greenBar;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
