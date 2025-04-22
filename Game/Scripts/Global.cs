using Godot;

namespace CrankUp
{
    public partial class Global : Node
    {
        public override void _Ready()
        {
            SaveSystem.LoadGame();
        }
    }
}
