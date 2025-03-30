using Godot;
using System;

namespace CrankUp
{
	public partial class Settings : Window
	{
		[Signal] public delegate void LanguageChangedEventHandler(string langCode);
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private TextureButton _fiButton = null;
		[Export] private TextureButton _enButton = null;
		private SettingsData _data = null;


		public override void _Ready()
		{
			base._Ready();

			// Lataa asetukset tiedostosta.
			_data = LoadSettings();
			ApplyData(_data);

			_fiButton = GetNodeOrNull<TextureButton>("Buttons/FI");
            _fiButton.Pressed += FiButtonPressed;

			_enButton = GetNodeOrNull<TextureButton>("Buttons/EN");
            _enButton.Pressed += EnButtonPressed;

			TextureButton exitButton = GetNode<TextureButton>("ExitButton");
			if (exitButton == null)
			{
				GD.PrintErr("[ERROR] ExitButton not found in Settings.tscn");
			}
			else
			{
				exitButton.Pressed += ExitButtonPressed;
			}

			if (_fiButton != null)
			{
				_fiButton.Connect("pressed",
				new Callable(this, nameof(FiButtonPressed)));
			}

			if (_enButton != null)
			{
				_enButton.Connect("pressed",
				new Callable(this, nameof(EnButtonPressed)));
			}
		}

		private void FiButtonPressed()
		{
			GD.Print("FI Button Pressed");
			ChangeLanguage("fi");
		}

		private void EnButtonPressed()
		{
			GD.Print("EN Button Pressed");
			ChangeLanguage("en");
		}

		private bool ChangeLanguage(string LangCode)
		{
			if (_data == null)
			{
				return false;
			}
			_data.LangCode = LangCode;
			TranslationServer.SetLocale(LangCode);
			GD.Print($"Language changed to: {LangCode}");
			// tallennus

			return true;
		}

		private void ApplyData(SettingsData data)
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
			SetLanguage(data.LangCode);
		}

		public bool SaveSettings()
		{
			if (_data == null)
			{
				return false;
			}

			ConfigFile settingsConfig = new ConfigFile();
			settingsConfig.SetValue("Localization", "LangCode", _data.LangCode);
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

		private SettingsData LoadSettings()
		{
			SettingsData data = null;
			ConfigFile settingsConfig = new ConfigFile();
			if (settingsConfig.Load(Config.SettingsFile) == Error.Ok)
			{
				// Settings-tiedosto ladattiin onnistuneesti.
				data = new SettingsData
				{
					LangCode = (string)settingsConfig.GetValue("Localization", "LangCode", "en"),
					MasterVolume = (float)settingsConfig.GetValue("Audio", "MasterVolume", -6.0f),
					MusicVolume = (float)settingsConfig.GetValue("Audio", "MusicVolume", -6.0f),
					SfxVolume = (float)settingsConfig.GetValue("Audio", "SfxVolume", -6.0f)
				};
			}
			else
			{
				// Asetustiedostoa ei löydetty, luodaan oletusasetukset.
				/*data = SettingsData.CreateDefaults();*/
			}

			return data;
		}

		/*public bool SetVolume(string busName, float volumeDB)
		{
			if (_data == null)
			{
				return false;
			}

			int busIndex = AudioServer.GetBusIndex(busName);
			if (busIndex < 0)
			{
				GD.PrintErr($"Bus '{busName}' not found.");
				return false;
			}

			AudioServer.SetBusVolumeDb(busIndex, volumeDB);

			switch (busName)
			{
				case "Master":
					_data.MasterVolume = volumeDB;
					break;
				case "Music":
					_data.MusicVolume = volumeDB;
					break;
				case "SFX":
					_data.SfxVolume = volumeDB;
					break;
				default:
					GD.PrintErr($"Unknown bus '{busName}'.");
					break;
			}

			return true;
		}

		public bool GetVolume(string busName, out float volumeDB)
		{
			int busIndex = AudioServer.GetBusIndex(busName);
			if (busIndex < 0)
			{
				GD.PrintErr($"Bus '{busName}' not found.");
				volumeDB = float.NaN;
				return false;
			}

			volumeDB = AudioServer.GetBusVolumeDb(busIndex);
			return true;
		}

		public string GetLanguage()
		{
			return TranslationServer.GetLocale();
		}*/

		public bool SetLanguage(string langCode)
		{
			if (_data == null)
			{
				return false;
			}

			_data.LangCode = langCode;
			TranslationServer.SetLocale(langCode);

			// Välitä tieto kielen vaihtumisesta.
			EmitSignal(SignalName.LanguageChanged, langCode);

			_data.LangCode = langCode;

			return true;
		}

		public void ExitButtonPressed()
		{
			GD.Print("Exit Pressed");

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
	}
}
