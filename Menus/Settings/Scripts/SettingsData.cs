namespace CrankUp
{
    public class SettingsData
    {
        public string LangCode { get; set; }
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SfxVolume { get; set; }

        public static SettingsData CreateDefaults()
        {
            return new SettingsData
            {
                LangCode = "en",
                MasterVolume = -6.0f,
                MusicVolume = -6.0f,
                SfxVolume = -6.0f 
            };
        }
    }
}