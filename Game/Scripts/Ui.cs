using Godot;
using System;

public partial class Ui : Control
{
	private int timeLeft = 60;
	private Label timerLabel;
	private Timer countDownTimer;
	public override void _Ready() {
		timerLabel = GetNode<Label>("Label");
		countDownTimer = GetNode<Timer>("Timer");

		countDownTimer.WaitTime = 1.0f;
		countDownTimer.Autostart = true;
		countDownTimer.OneShot = false;
		countDownTimer.Start();

		countDownTimer.Timeout += OnTimerTimeout;
		UpdateTimerLabel();
	}

	private void OnTimerTimeout() {
		if (timeLeft > 0)
		{
			timeLeft -= 1;
			UpdateTimerLabel();
		}
	}

	private void UpdateTimerLabel() {
		if (timerLabel != null)
			timerLabel.Text = timeLeft.ToString();
	}
}
