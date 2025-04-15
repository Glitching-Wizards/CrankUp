using Godot;
using System;

namespace CrankUp
{
    public partial class Credits2 : Window
    {
        [Export] private AudioStream clickSound;

        public Window PreviousWindow { get; set; }

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

        public void BackButtonPressed()
        {
            GD.Print("Back Pressed");
            AudioManager.PlaySound(clickSound);
            this.Hide();
            PreviousWindow?.PopupCentered(); // Show the previous window
        }

        public void ExitButtonPressed()
        {
            GD.Print("Exit Pressed");
            AudioManager.PlaySound(clickSound);
            this.Hide();
        }
    }
}