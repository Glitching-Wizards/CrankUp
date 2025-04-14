using Godot;
using System;

namespace CrankUp;

/// <summary>
/// UI control script for handling horizontal movement of the claw base, grabbing blocks, and pausing the game.
/// </summary>
public partial class ControlsRightUi : Control
{
    /// <summary>
    /// Path to the pause/settings window scene.
    /// </summary>
    [Export] private string _pauseScenePath = "res://Menus/Settings/Scenes/Pause.tscn";

    private ClawHead clawHead;
    private ClawBase clawBase;
    private Window pauseWindow;
    private TextureButton grabButton;
    private TextureButton pauseButton;
    private VSlider moveSlider;
    private float moveSliderValue;
    private Vector2 movementDirection = Vector2.Zero;

    /// <summary>
    /// Called when the node enters the scene tree. Initializes references and connects UI signals.
    /// </summary>
    public override void _Ready()
    {
<<<<<<< HEAD
        // Find the Grab button
=======
        // Find the Grab button safely
>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
        grabButton = GetNodeOrNull<TextureButton>("Panel/Grab");
        if (grabButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Grab' not found in ControlsLeftUi!");
            return;
        }

        // Find the move slider
        moveSlider = GetNodeOrNull<VSlider>("Panel/MoveSlider");
        if (moveSlider == null)
        {
            GD.PrintErr("[ERROR] VSlider 'moveSlider' not found in ControlsRightUi!");
            return;
        }

        // Find the Pause button
        pauseButton = GetNodeOrNull<TextureButton>("Panel/PauseButton");
        if (pauseButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Settings' not found in ControlsRightUi!");
        }

        // Load and prepare the pause menu window
        PackedScene pauseScene = (PackedScene)GD.Load(_pauseScenePath);
    	pauseWindow = (Window)pauseScene.Instantiate();
    	AddChild(pauseWindow);
    	pauseWindow.Hide();

        // Find ClawHead dynamically in the scene
        clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead not found in the scene tree!");
            return;
        }

        // Find ClawBase dynamically in the scene
        clawBase = GetTree().Root.FindChild("ClawBase", true, false) as ClawBase;
        if (clawBase == null)
        {
            GD.PrintErr("[ERROR] ClawBase not found in the scene tree!");
            return;
        }

        // Connect button and slider signals
        grabButton.Pressed += OnGrabPressed;
        pauseButton.Pressed += OnPausePressed;
        moveSlider.ValueChanged += OnSliderValueChanged;
        moveSlider.DragEnded += OnSliderReleased;
    }

    /// <summary>
    /// Triggered when the grab button is pressed. Instructs the claw head to grab or drop a block.
    /// </summary>
    private void OnGrabPressed()
    {
        if (clawHead != null) clawHead.GrabBlock();
    }

    /// <summary>
    /// Called when the slider value changes. Sets the horizontal movement direction and speed.
    /// </summary>
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

    /// <summary>
    /// Called when the slider is released. Resets movement.
    /// </summary>
    private void OnSliderReleased(bool ended)
    {
        if (ended)
        {
            moveSlider.Value = 0;
            moveSliderValue = 0;
            movementDirection = Vector2.Zero;
        }
    }

    /// <summary>
    /// Triggered when the pause button is pressed. Pauses the current level and displays the pause menu.
    /// </summary>
    private void OnPausePressed()
    {
        Node currentScene = GetTree().CurrentScene;

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

    /// <summary>
    /// Frame update loop. Applies movement to the claw base based on slider input.
    /// </summary>
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
