using Godot;
using System;

namespace CrankUp
{
	public partial class Settings : Window
	{
		[Signal] public delegate void LanguageChangedEventHandler(string Language);
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private TextureButton _fiButton = null;
		[Export] private TextureButton _enButton = null;
		[Export] private AudioStream clickSound;
		[Export] private HSlider _masterSlider;
		[Export] private TextureRect _masterIcon;
		[Export] private HSlider _musicSlider;
		[Export] private TextureRect _musicIcon;
		[Export] private HSlider _sfxSlider;
		[Export] private TextureRect _sfxIcon;
		[Export] private Texture2D _speakerIcon;
		[Export] private Texture2D _muteIcon;
		private TextureRect _musicMute1;
		private TextureRect _musicMute2;
		private TextureRect _musicMute3;


		private GameData _data = null;
		private string _originalLanguage = null;
		private string Language;

		public override void _Ready()
		{
			base._Ready();

			_data = LoadSettings();

			_musicMute1 = GetNodeOrNull<TextureRect>("Buttons/Music1/Mute");
			_musicMute2 = GetNodeOrNull<TextureRect>("Buttons/Music2/Mute");
			_musicMute3 = GetNodeOrNull<TextureRect>("Buttons/Music3/Mute");

			_fiButton = GetNodeOrNull<TextureButton>("Buttons/FI");
			_enButton = GetNodeOrNull<TextureButton>("Buttons/EN");

			_fiButton.Pressed += FiButtonPressed;
			_enButton.Pressed += EnButtonPressed;
			TextureButton exitButton = GetNode<TextureButton>("ExitButton");
			exitButton.Pressed += ExitButtonPressed;

			_masterSlider.ValueChanged -= OnMasterSliderChanged;
			_musicSlider.ValueChanged -= OnMusicSliderChanged;
			_sfxSlider.ValueChanged -= OnSfxSliderChanged;

			_masterSlider.Value = Mathf.DbToLinear(_data.MasterVolume);
			_musicSlider.Value = Mathf.DbToLinear(_data.MusicVolume);
			_sfxSlider.Value = Mathf.DbToLinear(_data.SfxVolume);

			UpdateIcon(_masterIcon, _masterSlider.Value);
			UpdateIcon(_musicIcon, _musicSlider.Value);
			UpdateIcon(_sfxIcon, _sfxSlider.Value);

			_masterSlider.ValueChanged += OnMasterSliderChanged;
			_musicSlider.ValueChanged += OnMusicSliderChanged;
			_sfxSlider.ValueChanged += OnSfxSliderChanged;

			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), _data.MasterVolume);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), _data.MusicVolume);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), _data.SfxVolume);

			ApplyData(_data);
		}


		public void Initialized()
		{
			_originalLanguage = GetLanguage();
		}

		/// <summary>
		/// Changes the language to finnish.
		/// </summary>
		private void FiButtonPressed()
		{
			ChangeLanguage("fi");
			AudioManager.PlaySound(clickSound);
		}

		/// <summary>
		/// Changes the language to english.
		/// </summary>
		private void EnButtonPressed()
		{
			ChangeLanguage("en");
			AudioManager.PlaySound(clickSound);
		}

		/// <summary>
		/// Changes the language of the game.
		/// </summary>
		/// <param name="Language"></param>
		private bool ChangeLanguage(string Language)
		{
			if (_data == null)
			{
				GD.PrintErr("Error: Settings data is null.");
				return false;
			}
			_data.Language = Language;

			TranslationServer.SetLocale(Language);

			SaveSettings();

			return true;
		}

		/// <summary>
		/// Applies the data to the settings.
		/// </summary>
		/// <param name="data"></param>
		private void ApplyData(GameData data)
		{
			if (data == null)
			{
				return;
			}
			SetLanguage(data.Language);
		}

		public bool SaveSettings()
		{
			if (_data == null) return false;

			SaveSystem.SaveGame();
			return true;
		}


		private GameData LoadSettings()
		{
			var data = SaveSystem.GetGameData();
			if (data == null)
			{
				data = GameData.CreateDefaults();
				SaveSystem.SaveGame();
			}
			return data;
		}
		private void SetVolume(HSlider slider, TextureRect icon, string busName, float dbVolume)
		{
			int busIndex = AudioServer.GetBusIndex(busName);
			if (busIndex >= 0)
			{
				AudioServer.SetBusVolumeDb(busIndex, dbVolume);
				slider.Value = Mathf.DbToLinear(dbVolume);
				UpdateIcon(icon, slider.Value);
			}
		}

		private void UpdateIcon(TextureRect icon, double value)
		{
			if (icon == null)
			{
				GD.PrintErr("[UpdateIcon] TextureRect is null!");
				return;
			}

			icon.Texture = value <= 0.01 ? _muteIcon : _speakerIcon;

			bool isMuted = value <= 0.01;
			if (_musicMute1 != null) _musicMute1.Visible = isMuted;
			if (_musicMute2 != null) _musicMute2.Visible = isMuted;
			if (_musicMute3 != null) _musicMute3.Visible = isMuted;
		}


		private void OnMasterSliderChanged(double value)
		{
			UpdateIcon(_masterIcon, value);
			var db = Mathf.LinearToDb((float)value);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), db);
			_data.MasterVolume = db;
			SaveSettings();
			GD.Print($"[Slider] Music volume changed: {db}dB");

		}

		private void OnMusicSliderChanged(double value)
		{
			UpdateIcon(_musicIcon, value);
			var db = Mathf.LinearToDb((float)value);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), db);
			_data.MusicVolume = db;
			SaveSettings();
			GD.Print($"[Slider] Music volume changed: {db}dB");

		}

		private void OnSfxSliderChanged(double value)
		{
			UpdateIcon(_sfxIcon, value);
			var db = Mathf.LinearToDb((float)value);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), db);
			_data.SfxVolume = db;
			SaveSettings();
			GD.Print($"[Slider] Music volume changed: {db}dB");

		}

		private void ToggleMute(HSlider slider, TextureRect icon, string busName, ref float volumeDb)
		{
			if (slider.Value > 0.01)
			{
				slider.Value = 0;
			}
			else
			{
				slider.Value = 1;
			}

			float db = Mathf.LinearToDb((float)slider.Value);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(busName), db);
			UpdateIcon(icon, slider.Value);

			volumeDb = db;
			SaveSettings();
		}

		/// <summary>
		/// Get the current language of the game.
		/// </summary>
		public string GetLanguage()
		{
			return TranslationServer.GetLocale();
		}

		/// <summary>
		/// Set the language of the game.
		/// </summary>
		/// <param name="Language"></param>
		public bool SetLanguage(string Language)
		{
			if (_data == null)
			{
				return false;
			}

			_data.Language = Language;
			TranslationServer.SetLocale(Language);

			// Käyttöliittymän päivitys
			UpdateUIForNewLanguage();
			// Välitä tieto kielen vaihtumisesta.
			EmitSignal(SignalName.LanguageChanged, Language);

			_data.Language = Language;

			return true;
		}

		/// <summary>
		/// Closes the settings window and returns to the main menu or levels.
		/// </summary>
		public void ExitButtonPressed()
		{
			GD.Print("Exit Pressed");
			AudioManager.PlaySound(clickSound);

			Node currentScene = GetTree().CurrentScene;

			if (currentScene != null && currentScene.Name == "MainMenu"
			|| currentScene.Name == "Levels" || currentScene.Name == "Level1"
			|| currentScene.Name == "Level2" || currentScene.Name == "Level3"
			|| currentScene.Name == "Level4" || currentScene.Name == "Level5")
			{
				this.Hide();
			}
			else
			{
				GetTree().ChangeSceneToFile(_levelsScenePath);
			}
		}

		private void UpdateUIForNewLanguage()
		{
			// Tässä voit päivittää UI:n tekstit, jos ne eivät ole automaattisesti päivittyneet.
			// Esimerkiksi:
			var labels = GetTree().GetNodesInGroup("ui_labels");
			foreach (var label in labels)
			{
				if (label is Label)
				{
					(label as Label).Text = (label as Label).Text;  // Päivitä teksti
				}
			}
		}

		public void OnVolumeChanged(float volume)
		{
			SaveSystem.GetGameData().MasterVolume = volume;
			SaveSystem.SaveGame();
		}

		/// <summary>
		/// This function is called when the language is changed.
		/// </summary>
		/// <param name="lang"></param>
		public void OnLanguageChanged(string lang)
		{
			SaveSystem.GetGameData().Language = lang;
			SaveSystem.SaveGame();
		}
	}
}