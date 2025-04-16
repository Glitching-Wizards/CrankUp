using Godot;
using System;

namespace CrankUp
{
    public partial class Credits : Window
    {
        [Export] private string _secondScenePath = "res://Menus/Settings/Scenes/Credits2.tscn";
        [Export] private AudioStream clickSound;

        private Window secondWindow;

        /// <summary>
        /// Loads the windows and checks when the buttons are pressed and calls the methods for them.
        /// </summary>
        public override void _Ready()
        {
            // Loads the second scene
            PackedScene secondScene = GD.Load<PackedScene>(_secondScenePath);
            secondWindow = (Window)secondScene.Instantiate();
            AddChild(secondWindow);
            secondWindow.Hide();

            // Saves the reference to the first window
            var instance = secondScene.Instantiate();
            if (instance is Credits2 secondCredits)
            {
                secondCredits.PreviousWindow = this;
                secondWindow = secondCredits;
                AddChild(secondCredits);
                secondCredits.Hide();
            }

            TextureButton nextButton = GetNodeOrNull<TextureButton>("Buttons/NextButton");
            nextButton.Pressed += NextButtonPressed;

            TextureButton exitButton = GetNodeOrNull<TextureButton>("Buttons/ExitButton");
            exitButton.Pressed += ExitButtonPressed;
        }

        /// <summary>
        /// When the next button is pressed, it hides the current window and shows the second window.
        /// It also plays the click sound.
        /// </summary>
        public void NextButtonPressed()
        {
            GD.Print("Next Pressed");
            AudioManager.PlaySound(clickSound);
            secondWindow.Popup();
            this.Hide();
        }

        /// <summary>
        /// When the exit button is pressed, it hides the current window and plays the click sound.
        /// </summary>
        public void ExitButtonPressed()
        {
            GD.Print("Exit Pressed");
            AudioManager.PlaySound(clickSound);
            this.Hide();
        }

    }
}