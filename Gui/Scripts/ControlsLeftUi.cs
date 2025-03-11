using Godot;
using System;

namespace CrankUp;
public partial class ControlsLeftUi : Control
{
    private ClawHead clawHead;
    private TextureButton grabButton;
    private VSlider moveSlider;
    private float moveSliderValue;
    private Vector2 movementDirection = Vector2.Zero;

    public override void _Ready()
    {
        // Find the Grab button safely
        grabButton = GetNodeOrNull<TextureButton>("Panel/Grab");
        if (grabButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Grab' not found in ControlsLeftUi!");
            return;
        }

        // Find the moveSlider safely
        moveSlider = GetNodeOrNull<VSlider>("Panel/MoveSlider");
        if (moveSlider == null)
        {
            GD.PrintErr("[ERROR] VSlider 'moveSlider' not found in ControlsLeftUi!");
            return;
        }

        // Find ClawHead dynamically
        clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead not found in the scene tree!");
            return;
        }

        moveSliderValue = (float)moveSlider.Value;

        // Connect button signals
        grabButton.Pressed += OnGrabPressed;
        moveSlider.ValueChanged += OnSliderValueChanged;
    }

    private void OnGrabPressed()
    {
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead reference is missing!");
            return;
        }
        clawHead.GrabBlock();
    }

    private void OnSliderValueChanged(double value)
    {
        moveSliderValue = (float)value;

        if (moveSliderValue > 0)
            movementDirection = Vector2.Up;
        else if (moveSliderValue < 0)
            movementDirection = Vector2.Down;
        else
            movementDirection = Vector2.Zero;
    }

    public override void _Process(double delta)
    {
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead reference is missing!");
            return;
        }
        clawHead.Move(movementDirection, moveSliderValue, delta);
    }
}
