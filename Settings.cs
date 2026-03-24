using System.IO;
using System.Text.Json;

namespace RhytmXT;

public class Settings
{
    public float backgroundDim { get; set; }
    public float rectangleDim { get; set; }

    public static Settings Load()
    {
        using (FileStream fs = new("settings.json", FileMode.OpenOrCreate))
        {
            if (fs.Length == 0)
            {
                return new Settings { backgroundDim = 0.5f, rectangleDim = 0.8f };
            }

            Settings settings = JsonSerializer.Deserialize<Settings>(fs);
            return settings ?? new Settings();
        }
    }
}