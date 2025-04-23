using Godot;

public partial class AudioManager : Node
{
	private static AudioStreamPlayer _truckPlayer;
	private static AudioStreamPlayer _conveyorPlayer;
	private static AudioStreamPlayer _sfxPlayer;
	private static AudioStreamPlayer _musicPlayer;

		public override void _Ready()
		{
			ProcessMode = ProcessModeEnum.Always;

			_truckPlayer = new AudioStreamPlayer { Name = "TruckPlayer" };
			AddChild(_truckPlayer);

			_conveyorPlayer = new AudioStreamPlayer { Name = "ConveyorPlayer" };
			AddChild(_conveyorPlayer);

			_sfxPlayer = new AudioStreamPlayer { Name = "SFXPlayer" };
			AddChild(_sfxPlayer);

			_musicPlayer = new AudioStreamPlayer { Name = "MusicPlayer" };
			AddChild(_musicPlayer);

			_musicPlayer.Bus = "Music";
			_sfxPlayer.Bus = "SFX";
			_truckPlayer.Bus = "SFX";
			_conveyorPlayer.Bus = "SFX";

			if (AudioServer.GetBusIndex("SFX") == -1)
			 GD.PrintErr("SFX audio bus not found!");

			 GD.Print("Truck bus: ", _truckPlayer.Bus);
			GD.Print("SFX bus: ", _sfxPlayer.Bus);
			GD.Print("Music bus: ", _musicPlayer.Bus);

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
			GD.Print($"Playing SFX: VolumeDb={_sfxPlayer.VolumeDb}, Bus={_sfxPlayer.Bus}, Stream={_sfxPlayer.Stream}");

		}
	}

	/// <summary>
	/// Plays background music. Prevents restart if the same music is already playing.
	/// </summary>
	public static void PlayMusic(AudioStream music)
	{
		_musicPlayer.ProcessMode = ProcessModeEnum.Always;
		
		if (_musicPlayer == null || music == null)
			return;

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

	public static void SetMasterVolume(float volumeDb)
	{
		GD.Print($"[AudioManager] Setting Master Volume to {volumeDb}dB");

		int masterIndex = AudioServer.GetBusIndex("Master");
		if (masterIndex != -1)
			AudioServer.SetBusVolumeDb(masterIndex, volumeDb);
	}
}
