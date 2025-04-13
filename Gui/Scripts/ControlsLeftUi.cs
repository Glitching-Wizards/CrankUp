using Godot;
using System;

namespace CrankUp;
public partial class ControlsLeftUi : Control
{
    private ClawHead clawHead;
    private TextureButton rotateButton;
    private VSlider moveSlider;
    private float moveSliderValue;
    private Vector2 movementDirection = Vector2.Zero;
    [Export] private AudioStream clickSound;

    public override void _Ready()
    {
        AddToGroup("multi_sliders");

        rotateButton = GetNodeOrNull<TextureButton>("Panel/Rotate");
        if (rotateButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Rotate' not found in ControlsLeftUi!");
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
        rotateButton.Pressed += OnRotatePressed;
        moveSlider.ValueChanged += OnSliderValueChanged;
        moveSlider.DragEnded += OnSliderReleased;
    }

    private void OnRotatePressed()
    {
        if (clawHead != null) clawHead.RotateBlock();
        AudioManager.PlaySound(clickSound);
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

    private void OnSliderReleased(bool ended)
    {
        if (ended)
        {
            moveSlider.Value = 0;
            moveSliderValue = 0;
            movementDirection = Vector2.Zero;
        }
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
