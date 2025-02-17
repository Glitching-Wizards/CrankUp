using Godot;
using System;

namespace CrankUp
{
    public partial class Level1 : Node2D
    {
        [Export] private string _nappulatScenePath = "res://Level1/Scenes/buttons.tscn";
        [Export] private string _sliderScenePath = "res://Level1/Scenes/Slider.tscn";
        [Export] private string _conveyorScenePath = "res://Level1/Scenes/Conveyor.tscn";
        [Export] private string _greenBScenePath = "res://Level1/Scenes/GreenBlock.tscn";
        [Export] private string _yellowBScenePath = "res://Level1/Scenes/YellowBlock.tscn";
        [Export] private string _rightConsolScenePath = "res://Level1/Scenes/right_consol.tscn";
        [Export] private string _leftConsolScenePath = "res://Level1/Scenes/left_consol.tscn";

        private PackedScene _nappulatScene = null;
        private PackedScene _sliderScene = null;
        private PackedScene _conveyorScene = null;
        private PackedScene _greenBScene = null;
        private PackedScene _yellowBScene = null;
        private PackedScene _rightConsolScene = null;
        private PackedScene _leftConsolScene = null;

        public override void _Ready()
        {
            _nappulatScene = ResourceLoader.Load<PackedScene>(_nappulatScenePath);
            _sliderScene = ResourceLoader.Load<PackedScene>(_sliderScenePath);
            _conveyorScene = ResourceLoader.Load<PackedScene>(_conveyorScenePath);
            _greenBScene = ResourceLoader.Load<PackedScene>(_greenBScenePath);
            _yellowBScene = ResourceLoader.Load<PackedScene>(_yellowBScenePath);
            _rightConsolScene = ResourceLoader.Load<PackedScene>(_rightConsolScenePath);
            _leftConsolScene = ResourceLoader.Load<PackedScene>(_leftConsolScenePath);

            GettingScene(_nappulatScene, new Vector2(0, 0));
            GettingScene(_sliderScene, new Vector2(70, 350));
            GettingScene(_sliderScene, new Vector2(1215, 350));
            GettingScene(_conveyorScene, new Vector2(650, 650));
            GettingScene(_greenBScene, new Vector2(500, 600));
            GettingScene(_yellowBScene, new Vector2(400, 600));
            GettingScene(_rightConsolScene, new Vector2(0, 0));
            GettingScene(_leftConsolScene, new Vector2(0, 0));
        }

        // Metodi, joka hakee scenen
        public void GettingScene(PackedScene scene, Vector2 position)
        {
            if (scene != null)
            {
                Node sceneInstance = scene.Instantiate();
                AddChild(sceneInstance);

                if (sceneInstance is Node2D node2D)
                {
                    node2D.Position = position;
                }
            }
        }
    }
}
