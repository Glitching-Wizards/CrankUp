using Godot;
using System;

namespace CrankUp;
public partial class ControlsRightUi : Control
{
    [Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
    private ClawHead clawHead;
    private ClawBase clawBase;
    private Window settingsWindow;
    private TextureButton releaseButton;
    private TextureButton settingsButton;
    private VSlider moveSlider;
    private float moveSliderValue;
    private Vector2 movementDirection = Vector2.Zero;

    public override void _Ready()
    {
        releaseButton = GetNodeOrNull<TextureButton>("Panel/Release");
        if (releaseButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Release' not found in ControlsRightUi!");
            return;
        }

        settingsButton = GetNodeOrNull<TextureButton>("Panel/Settings");
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
    	settingsWindow.Visible = false;

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
        releaseButton.Pressed += OnDropPressed;
        settingsButton.Pressed += OnSettingsPressed;
        moveSlider.ValueChanged += OnSliderValueChanged;
    }

    private void OnDropPressed()
    {
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead reference is missing!");
            return;
        }
        clawHead.DropBlock();
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
        clawBase.Move(movementDirection, moveSliderValue, delta);
    }
}
