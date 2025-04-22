using Godot;
using System;

public partial class Tutorial : Control
{
    [Export] public string FullText;

    [Export] public float LetterDelay = 0.05f; // sekuntia / merkki

    private RichTextLabel _label;
    private Timer _timer;
    private int _currentIndex = 0;

    public override void _Ready()
    {
        _label = GetNode<RichTextLabel>("SpeechBubble/RichTextLabel");

        // Luo ja lisää Timer node ohjelmallisesti
        _timer = new Timer();
        _timer.WaitTime = LetterDelay;
        _timer.OneShot = false;
        AddChild(_timer);
        _timer.Timeout += OnTimerTimeout;


        var button = GetNode<Button>("Button");
        button.Pressed += StartTyping;

        // Ei aloiteta vielä – odotetaan nappipainallusta
        _label.Text = "";
    }

    private void ButtonPressed()
    {
        StartTyping();
    }

    public void StartTyping()
    {
        _label.Text = "";
        _currentIndex = 0;
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        if (_currentIndex < FullText.Length)
        {
            _label.Text += FullText[_currentIndex];
            _currentIndex++;
        }
        else
        {
            _timer.Stop(); // valmis
        }
    }
}
