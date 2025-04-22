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

		private GameData _data = null;
		private string _originalLanguage = null;
		private string Language;

		public override void _Ready()
		{
			base._Ready();

			// Lataa asetukset tiedostosta.
			_data = LoadSettings();

			_fiButton = GetNodeOrNull<TextureButton>("Buttons/FI");
			_fiButton.Pressed += FiButtonPressed;

			_enButton = GetNodeOrNull<TextureButton>("Buttons/EN");
			_enButton.Pressed += EnButtonPressed;

			TextureButton exitButton = GetNode<TextureButton>("ExitButton");
			exitButton.Pressed += ExitButtonPressed;

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

		private void FiButtonPressed()
		{
			ChangeLanguage("fi");
			AudioManager.PlaySound(clickSound);
			_fiButton.Disabled = true;
			_enButton.Disabled = false;
		}

		private void EnButtonPressed()
		{
			ChangeLanguage("en");
			AudioManager.PlaySound(clickSound);
			_enButton.Disabled = true;
			_fiButton.Disabled = false;
		}

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

		private void ApplyData(GameData data)
		{
			if (data == null)
			{
				return;
			}
			/*
						// Aseta äänenvoimakkuudet.
						SetVolume("Master", data.MasterVolume);
						SetVolume("Music", data.MusicVolume);
						SetVolume("SFX", data.SfxVolume); */

			// Aseta kieli.
			SetLanguage(data.Language);
		}

		public bool SaveSettings()
		{
			if (_data == null)
			{
				return false;
			}

			ConfigFile settingsConfig = new ConfigFile();
			settingsConfig.SetValue("Localization", "Language", _data.Language);
			settingsConfig.SetValue("Audio", "MasterVolume", _data.MasterVolume);
			settingsConfig.SetValue("Audio", "MusicVolume", _data.MusicVolume);
			settingsConfig.SetValue("Audio", "SfxVolume", _data.SfxVolume);

			if (settingsConfig.Save(Config.SettingsFile) != Error.Ok)
			{
				GD.PrintErr("Failed to save settings.");
				return false;
			}

			return true;
		}

		private GameData LoadSettings()
		{
			GameData data = null;

			ConfigFile settingsConfig = new ConfigFile();
			if (settingsConfig.Load(Config.SettingsFile) == Error.Ok)
			{
				data = new GameData
				{
					Language = (string)settingsConfig.GetValue("Localization", "Language", "en"),
					MasterVolume = (float)settingsConfig.GetValue("Audio", "MasterVolume", -6.0f),
					MusicVolume = (float)settingsConfig.GetValue("Audio", "MusicVolume", -6.0f),
					SfxVolume = (float)settingsConfig.GetValue("Audio", "SfxVolume", -6.0f),
				};
			}
			else
			{
				data = GameData.CreateDefaults();
				SaveSettings();
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
			icon.Texture = value <= 0.01 ? _muteIcon : _speakerIcon;
		}

		private void OnMasterSliderChanged(double value)
		{
			UpdateIcon(_masterIcon, value);
			var db = Mathf.LinearToDb((float)value);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), db);
			_data.MasterVolume = db;
			SaveSettings();
		}

		private void OnMusicSliderChanged(double value)
		{
			UpdateIcon(_musicIcon, value);
			var db = Mathf.LinearToDb((float)value);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), db);
			_data.MusicVolume = db;
			SaveSettings();
		}

		private void OnSfxSliderChanged(double value)
		{
			UpdateIcon(_sfxIcon, value);
			var db = Mathf.LinearToDb((float)value);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), db);
			_data.SfxVolume = db;
			SaveSettings();
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

		public string GetLanguage()
		{
			return TranslationServer.GetLocale();
		}

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

		public void OnLanguageChanged(string lang)
		{
			SaveSystem.GetGameData().Language = lang;
			SaveSystem.SaveGame();
		}
	}
}
