using Godot;
using System;

namespace CrankUp
{
    public partial class Credits2 : Window
    {
        [Export] private AudioStream clickSound;

        public Window PreviousWindow { get; set; }

        /// <summary>
        /// Checks when the buttons are pressed and calls the methods for them.
        /// </summary>
        public override void _Ready()
        {
            TextureButton backButton = GetNodeOrNull<TextureButton>("Buttons/BackButton");
            if (backButton == null)
            {
                GD.PrintErr("[ERROR] BackButton not found in SoundCredits.tscn");
            }
            else
            {
                backButton.Pressed += BackButtonPressed;
            }

            TextureButton exitButton = GetNodeOrNull<TextureButton>("Buttons/ExitButton");
            if (exitButton == null)
            {
                GD.PrintErr("[ERROR] ExitButton not found in SoundCredits.tscn");
            }
            else
            {
                exitButton.Pressed += ExitButtonPressed;
            }
        }

        /// <summary>
        /// When the back button is pressed, it hides the current window and shows the previous window.
        /// It also plays the click sound.
        /// </summary>
        public void BackButtonPressed()
        {
            GD.Print("Back Pressed");
            AudioManager.PlaySound(clickSound);
            this.Hide();
            PreviousWindow?.PopupCentered(); // Show the previous window
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