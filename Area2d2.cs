using Godot;

public partial class Area2d2 : Area2D
{

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player)
        {
            GD.Print($"Body entered world boundary area: {body.Name}");
            GetTree().ReloadCurrentScene();
        }
    }
}