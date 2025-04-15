using Godot;
using System;

namespace CrankUp;

/// <summary>
/// UI control script for managing vertical movement and rotation of the claw head using UI elements like a slider and a button.
/// </summary>
public partial class ControlsLeftUi : Control
{
	[Export] private AudioStream clickSound;
	[Export] private AudioStream chainSound;
	private int timeLeft = 120;
	private Label timerLabel;
	private Timer countDownTimer;
	[Signal] public delegate void TimeRanOutEventHandler();

	/// <summary>
    /// Reference to the ClawHead node being controlled.
    /// </summary>
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

	/// <summary>
    /// Called when the node enters the scene tree. Initializes UI elements and connects their signals.
    /// </summary>
	public override void _Ready()
	{
		AddToGroup("multi_sliders");

		timerLabel = GetNode<Label>("Panel/Label");
		countDownTimer = GetNode<Timer>("Panel/Timer");

		countDownTimer.WaitTime = 1.0f;
		countDownTimer.Autostart = true;
		countDownTimer.OneShot = false;
		countDownTimer.Start();

		countDownTimer.Timeout += OnTimerTimeout;
		UpdateTimerLabel();

		AddToGroup("multi_sliders");

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

		moveSliderValue = (float)moveSlider.Value;

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
		AudioManager.PlaySound(clickSound);
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
}
