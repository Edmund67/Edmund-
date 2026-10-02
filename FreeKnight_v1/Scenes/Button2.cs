using Godot;
using System;
public partial class Button2 : Button
{
  
    private void _on_pressedthing()
    {
        
        GetTree().ChangeSceneToFile("res://level.tscn");
    }
}