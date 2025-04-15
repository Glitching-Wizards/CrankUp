// katsoo mitkä kaikki levelit on pelattu ja mitkä ei
using Godot;
using System;
using System.Collections.Generic;

namespace CrankUp
{
    /// <summary>
    /// Manages the level buttons in the level selection menu.
    /// </summary>
    public partial class LevelButtonManager : Control
    {
        [Export] public NodePath[] levelButtonsPaths;
        private GameData gameData;

        public override void _Ready()
        {
            gameData = SaveSystem.GetGameData();
            SetLevelButtons();
        }

        public void SetLevelButtons()
        {
            for (int i = 0; i < levelButtonsPaths.Length; i++)
            {
                var buttonNode = GetNode<LevelButton>(levelButtonsPaths[i]);

                int levelNumber = i + 1;

                buttonNode.Disabled = true;

                if (levelNumber < gameData.LevelProgress)
                {
                    buttonNode.Disabled = false;

                    // if level played and got at least 1 star -> show flag
                    if (gameData.LevelStars.ContainsKey(levelNumber) && gameData.LevelStars[levelNumber] > 0)
                    {
                        buttonNode.ShowFlagAndNumber();
                    }
                    else if (levelNumber == gameData.LevelProgress)
                    {
                        buttonNode.Disabled = false;
                    }
                }
            }
        }
    }
}