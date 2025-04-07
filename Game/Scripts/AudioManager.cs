using Godot;

public partial class AudioManager : Node
{
    private static AudioStreamPlayer _sfxPlayer;
    private static AudioStreamPlayer _musicPlayer;

    public override void _Ready()
    {
        // Initialize the sound effects player
        _sfxPlayer = new AudioStreamPlayer
        {
            Name = "SFXPlayer"
        };
        AddChild(_sfxPlayer);

        // Initialize the music player
        _musicPlayer = new AudioStreamPlayer
        {
            Name = "MusicPlayer",
            Loop = true // Music loops by default
        };
        AddChild(_musicPlayer);
    }

    /// <summary>
    /// Plays a one-shot sound effect.
    /// </summary>
    public static void PlaySound(AudioStream sound)
    {
        if (_sfxPlayer != null && sound != null)
        {
            _sfxPlayer.Stream = sound;
            _sfxPlayer.Play();
        }
    }

    /// <summary>
    /// Plays background music. Prevents restart if the same music is already playing.
    /// </summary>
    public static void PlayMusic(AudioStream music)
    {
        if (_musicPlayer == null || music == null)
            return;

        // Avoid restarting if the same music is already playing
        if (_musicPlayer.Stream == music && _musicPlayer.Playing)
            return;

        _musicPlayer.Stream = music;
        _musicPlayer.Play();
    }

    /// <summary>
    /// Stops the currently playing music.
    /// </summary>
    public static void StopMusic()
    {
        _musicPlayer?.Stop();
    }

    /// <summary>
    /// Optional: Set music volume (0.0 to 1.0)
    /// </summary>
    public static void SetMusicVolume(float volume)
    {
        if (_musicPlayer != null)
            _musicPlayer.VolumeDb = Mathf.LinearToDb(Mathf.Clamp(volume, 0f, 1f));
    }

    /// <summary>
    /// Optional: Set SFX volume (0.0 to 1.0)
    /// </summary>
    public static void SetSFXVolume(float volume)
    {
        if (_sfxPlayer != null)
            _sfxPlayer.VolumeDb = Mathf.LinearToDb(Mathf.Clamp(volume, 0f, 1f));
    }
}
