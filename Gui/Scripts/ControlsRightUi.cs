using Godot;
using System;

namespace CrankUp;
public partial class ControlsRightUi : Control
{
    [Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Pause.tscn";
    private ClawHead clawHead;
    private ClawBase clawBase;
    private Window settingsWindow;
    private TextureButton grabButton;
    private TextureButton settingsButton;
    private VSlider moveSlider;
    private float moveSliderValue;
    private Vector2 movementDirection = Vector2.Zero;

    public override void _Ready()
    {
        AddToGroup("multi_sliders");

        // Find the Grab button safely
        grabButton = GetNodeOrNull<TextureButton>("Panel/Grab");
        if (grabButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Grab' not found in ControlsLeftUi!");
            return;
        }

        settingsButton = GetNodeOrNull<TextureButton>("Panel/SettingsButton");
        if (settingsButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Settings' not found in ControlsRightUi!");
        }

        moveSlider = GetNodeOrNull<VSlider>("Panel/MoveSlider");
        if (moveSlider == null)
        {
            GD.PrintErr("[ERROR] VSlider 'moveSlider' not found in ControlsRightUi!");
            return;
        }

        PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
    	settingsWindow = (Window)settingsScene.Instantiate();
    	AddChild(settingsWindow);
    	settingsWindow.Hide();

        // Find ClawHead dynamically
        clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead not found in the scene tree!");
            return;
        }

        clawBase = GetTree().Root.FindChild("ClawBase", true, false) as ClawBase;
        if (clawBase == null)
        {
            GD.PrintErr("[ERROR] ClawBase not found in the scene tree!");
            return;
        }

        // Connect button signals
        grabButton.Pressed += OnGrabPressed;
        settingsButton.Pressed += OnSettingsPressed;
        moveSlider.ValueChanged += OnSliderValueChanged;
        moveSlider.DragEnded += OnSliderReleased;
    }

    private void OnGrabPressed()
    {
        if (clawHead != null) clawHead.GrabBlock();
    }

    private void OnSliderValueChanged(double value)
    {
        moveSliderValue = (float)value;

        if (moveSliderValue > 0)
            movementDirection = Vector2.Right;
        else if (moveSliderValue < 0)
            movementDirection = Vector2.Left;
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

    private void OnSettingsPressed()
    {
        settingsWindow.Popup();
    }

    public override void _Process(double delta)
    {
        if (clawBase == null)
        {
            GD.PrintErr("[ERROR] ClawBase reference is missing!");
            return;
        }

        if (Input.IsActionPressed(Config.MoveRightAction))
        {
            moveSliderValue = 35;
            moveSlider.Value = moveSliderValue;
            movementDirection = Vector2.Right;
        }
        else if (Input.IsActionPressed(Config.MoveLeftAction))
        {
            moveSliderValue = -35;
            moveSlider.Value = moveSliderValue;
            movementDirection = Vector2.Left;
        }
        else
        {
            moveSliderValue = 0;
            moveSlider.Value = moveSliderValue;
            movementDirection = Vector2.Zero;
        }

        if (Input.IsActionJustPressed(Config.GrabAction))
        {
            OnGrabPressed();
        }

        clawBase.Move(movementDirection, moveSliderValue, delta);
    }
}
