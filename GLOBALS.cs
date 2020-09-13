using System;
using Godot;
public class GLOBALS : Node
{
    public static int [] SlowTerrain = new []
    {0,1,2, 3,15,16,17,18,20,34,39,40};

    public static int CurrentLevel =0;
    public static string[] Levels = new string[] {"res://UI/TitleScreen.tscn", "res://Maps/Map01.tscn"};

    public static void Restart()
    {
        CurrentLevel =0;
    }
        


    public void NextLevel()
    {
        CurrentLevel+=1;
        if(CurrentLevel < Levels.Length)
        {
            GD.Print($"GetTree: {GetTree()}");
            GetTree().ChangeScene(Levels[CurrentLevel]);
        }
        else
        {
            Restart();
        }
    }
}

