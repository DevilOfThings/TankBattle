using Godot;
using System;

public class Pickup : Area2D
{

    public enum Items
    {
        Health,
        Ammo
    }

    Texture[] IconTextures;

    [Export]
    public Items Type = Items.Health;
    [Export]
    public Vector2 Amount = new Vector2(10,25);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        IconTextures = new Texture[] 
        {
            ResourceLoader.Load("res://Assets/wrench_white.png") as Texture,
            ResourceLoader.Load("res://Assets/ammo_machinegun.png") as Texture
        };
        GetNode<Sprite>("Icon").Texture = IconTextures[(int)Type]; 
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void _on_Pickup_body_entered(Node body)
    {
        GD.Print($"Body: {body}{body.Name} {Type}");

        switch(Type)
        {
            case Items.Health:
                GD.Print("_on_Pickup_body_entered health");
                if(body.HasMethod("Heal")) {
                     var type = body.GetType();
                    GD.Print($"{type}");
                    var heal = type.GetMethod("Heal");
                    GD.Print($"{heal}");
                    heal.Invoke(body, new object[] {new Random().Next((int)Amount.x,(int)Amount.y)});
                    QueueFree();
                }
            break;

            case Items.Ammo:
                GD.Print("_on_Pickup_body_entered ammo");
                
                (body as Tank).Ammo += new Random().Next((int)Amount.x,(int)Amount.y);
                QueueFree();
                
            break;
        }
    }
}
