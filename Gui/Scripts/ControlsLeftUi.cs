using Godot;
using System;

namespace CrankUp;

/// <summary>
/// UI control script for managing vertical movement and rotation of the claw head using UI elements like a slider and a button.
/// </summary>
public partial class ControlsLeftUi : Control
{
<<<<<<< HEAD
<<<<<<< HEAD
    /// <summary>
    /// Reference to the ClawHead node being controlled.
    /// </summary>
=======
>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
    private ClawHead clawHead;

    /// <summary>
    /// Button used to rotate the grabbed block.
    /// </summary>
    private TextureButton rotateButton;

    /// <summary>
    /// Slider used to control vertical movement of the claw head.
    /// </summary>
    private VSlider moveSlider;

    /// <summary>
    /// The current value of the move slider.
    /// </summary>
    private float moveSliderValue;

    /// <summary>
    /// The current movement direction of the claw (Up, Down, or None).
    /// </summary>
    private Vector2 movementDirection = Vector2.Zero;
    [Export] private AudioStream clickSound;

    /// <summary>
    /// Called when the node enters the scene tree. Initializes UI elements and connects their signals.
    /// </summary>
    public override void _Ready()
    {
        // Find the rotate button inside the UI hierarchy
        rotateButton = GetNodeOrNull<TextureButton>("Panel/Rotate");
        if (rotateButton == null)
        {
            GD.PrintErr("[ERROR] TextureButton 'Rotate' not found in ControlsLeftUi!");
        }

        // Find the vertical movement slider
        moveSlider = GetNodeOrNull<VSlider>("Panel/MoveSlider");
        if (moveSlider == null)
        {
            GD.PrintErr("[ERROR] VSlider 'moveSlider' not found in ControlsLeftUi!");
            return;
        }

        // Find the ClawHead node in the scene tree
        clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead not found in the scene tree!");
            return;
        }
=======
	[Export] private AudioStream clickSound;
	private int timeLeft = 15;
	private Label timerLabel;
	private Timer countDownTimer;
	[Signal] public delegate void TimeRanOutEventHandler();
	private ClawHead clawHead;
	private TextureButton rotateButton;
	private VSlider moveSlider;
	private float moveSliderValue;
	private Vector2 movementDirection = Vector2.Zero;

	public override void _Ready()
	{
		timerLabel = GetNode<Label>("Panel/Label");
		countDownTimer = GetNode<Timer>("Panel/Timer");

		countDownTimer.WaitTime = 1.0f;
		countDownTimer.Autostart = true;
		countDownTimer.OneShot = false;
		countDownTimer.Start();

		countDownTimer.Timeout += OnTimerTimeout;
		UpdateTimerLabel();

		AddToGroup("multi_sliders");
>>>>>>> 8a165564561152121a3ce3d167ff9dc1e441c839

		rotateButton = GetNodeOrNull<TextureButton>("Panel/Rotate");
		if (rotateButton == null)
		{
			GD.PrintErr("[ERROR] TextureButton 'Rotate' not found in ControlsLeftUi!");
		}

<<<<<<< HEAD
        // Connect UI signals to handlers
        rotateButton.Pressed += OnRotatePressed;
        moveSlider.ValueChanged += OnSliderValueChanged;
        moveSlider.DragEnded += OnSliderReleased;
    }

    /// <summary>
    /// Called when the rotate button is pressed. Tells the claw head to rotate the grabbed block.
    /// </summary>
    private void OnRotatePressed()
    {
        if (clawHead != null) clawHead.RotateBlock();
    }

    /// <summary>
    /// Called when the slider value changes. Updates the claw movement direction based on slider input.
    /// </summary>
    private void OnSliderValueChanged(double value)
    {
        moveSliderValue = (float)value;

        if (moveSliderValue > 0)
            movementDirection = Vector2.Up;
        else if (moveSliderValue < 0)
            movementDirection = Vector2.Down;
        else
            movementDirection = Vector2.Zero;
<<<<<<< HEAD
=======

>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
    }

    /// <summary>
    /// Called when the slider is released. Resets slider value and stops movement.
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
    /// Frame update loop. Moves the claw head each frame based on UI input.
    /// </summary>
    public override void _Process(double delta)
    {
        if (clawHead == null)
        {
            GD.PrintErr("[ERROR] ClawHead reference is missing!");
            return;
        }

        clawHead.Move(movementDirection, moveSliderValue, delta);
    }
=======
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

		AudioManager.PlaySound(clickSound);
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

	private void OnTimerTimeout()
		{
			if (timeLeft > 0)
			{
				timeLeft -= 1;
				UpdateTimerLabel();
				if (timeLeft == 0)
				{
					EmitSignal(SignalName.TimeRanOut);
				}
			}
		}

		private void UpdateTimerLabel()
		{
			if (timerLabel != null)
				timerLabel.Text = timeLeft.ToString();
		}
>>>>>>> 8a165564561152121a3ce3d167ff9dc1e441c839
}
