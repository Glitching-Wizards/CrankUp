using Godot;
using System;

namespace CrankUp;
public partial class ControlsLeftUi : Control
{
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
}
