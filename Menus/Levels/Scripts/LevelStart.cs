using Godot;
using System;

namespace CrankUp
{
    public partial class LevelStart : Window
    {
        [Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
        [Export] private string _level1ScenePath = "res://Game/Level1/Scenes/Level1.tscn";
        [Export] private string _level2ScenePath = "res://Game/Level2/Scenes/Level2.tscn";
        [Export] private string _level3ScenePath = "res://Game/Level3/Scenes/Level3.tscn";
        [Export] private string _level4ScenePath = "res://Game/Level4/Scenes/Level4.tscn";
        [Export] private string _level5ScenePath = "res://Game/Level5/Scenes/Level5.tscn";
        [Export] private AudioStream clickSound;
        [Export] private AudioStream truckSound;
        private string _selectedLevelScenePath = "";

        /// <summary>
        /// Checks when play or menu is pressed.
        /// </summary>
		public override void _Ready()
        {
            TextureButton PlayButton = GetNodeOrNull<TextureButton>("PlayButton");
            PlayButton.Pressed += PlayButtonPressed;

            TextureButton MenuButton = GetNodeOrNull<TextureButton>("MenuButton");
			MenuButton.Pressed += () => MenuButtonPressed();
        }

        /// <summary>
        /// Called when the menu button is pressed.
        /// Changes the scene to the levels menu ja plays click sound.
        /// </summary>
        public void MenuButtonPressed()
        {
            GetTree().ChangeSceneToFile(_levelsScenePath);
            AudioManager.PlaySound(clickSound);
        }

        /// <summary>
        /// Called when the play button is pressed.
        /// Changes the scene to the selected level and plays click and truck sound.
        /// </summary>
        public void PlayButtonPressed()
        {
            GD.Print("Play Pressed");
            AudioManager.PlaySound(clickSound);
            AudioManager.PlayTruckSound(truckSound);

            CallDeferred(nameof(ChangeScene));
		}

        /// <summary>
        /// Changes the scene to the selected level.
        /// </summary>
		private void ChangeScene()
		{
			GetTree().ChangeSceneToFile(_selectedLevelScenePath);
		}

        /// <summary>
        /// Sets the selected level based on the level number.
        /// </summary>
        /// <param name="level"></param>
        public void SetLevel(int level)
        {
            _selectedLevelScenePath = level switch
            {
                1 => _level1ScenePath,
                2 => _level2ScenePath,
                3 => _level3ScenePath,
                4 => _level4ScenePath,
                5 => _level5ScenePath,
                _ => ""
            };
        }
    }
}