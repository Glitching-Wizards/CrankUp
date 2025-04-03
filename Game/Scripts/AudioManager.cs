using Godot;

public partial class AudioManager : Node
{
    private static AudioStreamPlayer _player;

    public override void _Ready()
    {
        // 1. Create an AudioStreamPlayer node
        _player = new AudioStreamPlayer();

        // 2. Add it as a child of this node (AudioManager)
        AddChild(_player);
    }

    public static void PlaySound(AudioStream sound)
    {
        if (_player != null && sound != null)
        {
            _player.Stream = sound;
            _player.Play();
        }
    }
}

