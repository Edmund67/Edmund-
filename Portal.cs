using Godot;
using System;
using System.Threading.Tasks;


public partial class Portal : Area2D
{
	// Export the target scene path so you can set it uniquely in the Inspector
	[Export]
	public string TargetScenePath { get; set; }
	

	private void OnBodyEntered(Node2D body)
	{
		// Option A: Check if the colliding body is in the "Player" group
		if (body is Player)
		{
			ActivatePortal();
		}
		
		// Option B: Alternatively, check by class type
		// if (body is CharacterBody2D) { ActivatePortal(); }
	}

	private void ActivatePortal()
	{
		GD.Print("Portal Activated! Teleporting player...");

		if (!string.IsNullOrEmpty(TargetScenePath))
		{
			// Change to the new level or area
			GetTree().ChangeSceneToFile(TargetScenePath);
		}
		else
		{
			GD.PrintErr("Target scene path is empty!");
		}
	}
}

