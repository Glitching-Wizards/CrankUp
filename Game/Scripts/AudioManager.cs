using Godot;

public partial class AudioManager : Node
{
    private static AudioStreamPlayer _truckPlayer;
    private static AudioStreamPlayer _conveyorPlayer;
    private static AudioStreamPlayer _sfxPlayer;
    private static AudioStreamPlayer _musicPlayer;

    public override void _Ready()
    {
        // Initialize truck sound player
        _truckPlayer = new AudioStreamPlayer
        {
            Name = "TruckPlayer"
        };
        AddChild(_truckPlayer);

        // Initialize conveyor belt sound player
        _conveyorPlayer = new AudioStreamPlayer
        {
            Name = "ConveyorPlayer"
        };
        AddChild(_conveyorPlayer);

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
        };
        AddChild(_musicPlayer);
    }


    /// <summary>
    /// Plays the truck sound effect.
    /// </summary>
    /// <param name="sound"></param>
    public static void PlayTruckSound(AudioStream sound)
    {
        if (_truckPlayer != null && sound != null)
        {
            _truckPlayer.Stream = sound;
            _truckPlayer.Play();
        }
    }

    /// <summary>
    /// Plays the conveyor sound effect.
    /// </summary>
    /// <param name="sound"></param>
    public static void PlayConveyorSound(AudioStream sound)
    {
        if (_conveyorPlayer != null && sound != null)
        {
            _conveyorPlayer.Stream = sound;
            _conveyorPlayer.Play();
        }
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
