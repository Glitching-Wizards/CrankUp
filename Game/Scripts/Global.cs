using Godot;

namespace CrankUp
{
    public partial class Global : Node
    {
        public override void _Ready()
        {
            SaveSystem.LoadGame();
            ApplySavedVolumeSettings();
        }
        public static void ApplySavedVolumeSettings()
        {
            var data = CrankUp.SaveSystem.GetGameData();

            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), data.MasterVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), data.MusicVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), data.SfxVolume);

            GD.Print($"[Audio] Volumes applied - Master: {data.MasterVolume}, Music: {data.MusicVolume}, SFX: {data.SfxVolume}");
        }

    }
}
