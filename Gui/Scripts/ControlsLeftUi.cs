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

    public override void _Ready()
    {
        AddToGroup("multi_sliders");

        rotateButton = GetNodeOrNull<TextureButton>("Panel/Rotate");
        if (rotateButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Rotate' not found in ControlsLeftUi!");
        }

        // Find the moveSlider
        moveSlider = GetNodeOrNull<VSlider>("Panel/MoveSlider");
        if (moveSlider == null)
        {
            GD.PrintErr("[ERROR] VSlider 'moveSlider' not found in ControlsLeftUi!");
            return;
        }

        // Find ClawHead
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

        if (Input.IsActionPressed(Config.MoveUpAction))
        {
            moveSliderValue = 35;
            moveSlider.Value = moveSliderValue;
            movementDirection = Vector2.Up;
        }
        else if (Input.IsActionPressed(Config.MoveDownAction))
        {
            moveSliderValue = -35;
            moveSlider.Value = moveSliderValue;
            movementDirection = Vector2.Down;
        }
        else
        {
            moveSliderValue = 0;
            moveSlider.Value = moveSliderValue;
            movementDirection = Vector2.Zero;
        }

        if (Input.IsActionJustPressed(Config.RotateAction))
        {
            OnRotatePressed();
        }

        clawHead.Move(movementDirection, moveSliderValue, delta);
    }
}
