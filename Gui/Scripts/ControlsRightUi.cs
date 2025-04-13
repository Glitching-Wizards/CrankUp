using Godot;
using System;

namespace CrankUp;
public partial class ControlsRightUi : Control
{
    [Export] private string _pauseScenePath = "res://Menus/Settings/Scenes/Pause.tscn";
    [Export] private AudioStream clickSound;
    private ClawHead clawHead;
    private ClawBase clawBase;
    private Window pauseWindow;
    private TextureButton grabButton;
    private TextureButton pauseButton;
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

        moveSlider = GetNodeOrNull<VSlider>("Panel/MoveSlider");
        if (moveSlider == null)
        {
            GD.PrintErr("[ERROR] VSlider 'moveSlider' not found in ControlsRightUi!");
            return;
        }

        pauseButton = GetNodeOrNull<TextureButton>("Panel/PauseButton");
        if (pauseButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Settings' not found in ControlsRightUi!");
        }

        PackedScene pauseScene = (PackedScene)GD.Load(_pauseScenePath);
    	pauseWindow = (Window)pauseScene.Instantiate();
    	AddChild(pauseWindow);
    	pauseWindow.Hide();

        pauseButton.Pressed += OnPausePressed;

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
        pauseButton.Pressed += OnPausePressed;
        moveSlider.ValueChanged += OnSliderValueChanged;
        moveSlider.DragEnded += OnSliderReleased;
    }

    private void OnGrabPressed()
    {
        if (clawHead != null) clawHead.GrabBlock();
        AudioManager.PlaySound(clickSound);
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

    // työn alla
    private void OnPausePressed()
    {
        Node currentScene = GetTree().CurrentScene;
        AudioManager.PlaySound(clickSound);

        if (currentScene == null)
        {
            GD.PrintErr("[ERROR] No active scene found!");
            return;
        }

        switch (currentScene.Name)
        {
            case "Level1":
            case "Level2":
            case "Level3":
            case "Level4":
            case "Level5":
                GD.Print($"Pausing {currentScene.Name}...");
                GetTree().Paused = true;
                pauseWindow.Popup();
                break;
            default:
                GD.PrintErr("[ERROR] No active level found to pause!");
                break;
        }
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
