using Godot;

public partial class AudioManager : Node
{
    private static AudioStreamPlayer Player;

    public override void _Ready()
    {
        Player = new AudioStreamPlayer();
        AddChild(Player);
        Player.Stream = GD.Load<AudioStream>("res://button_click.wav");
    }

    public static void PlayButtonSound()
    {
        if (_player != null)
            _player.Play();
    }
}
