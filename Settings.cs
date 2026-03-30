using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace RhytmXT;

public class Settings
{
    public float backgroundDim { get; set; }
    public float rectangleDim { get; set; }
    public int localOffset { get; set; }
    public int keysCount { get; set; }
    public Dictionary<string, Dictionary<string, string>> keyBinds { get; set; }

    public static Settings Load()
    {
        using (FileStream fs = new("settings.json", FileMode.OpenOrCreate))
        {
            if (fs.Length == 0)
            {
                return new Settings 
                { 
                    backgroundDim = 0.5f, 
                    rectangleDim = 0.8f, 
                    localOffset = 0,
                    keysCount = 2,

                    keyBinds = new Dictionary<string, Dictionary<string, string>>
                    {
                        ["2keys"] = new Dictionary<string, string>
                        {
                            ["don1"] = "1",
                            ["don2"] = "2",
                            ["rim1"] = "[",
                            ["rim2"] = "]"
                        },

                        ["3keys"] = new Dictionary<string, string>
                        {
                            ["don1"] = "1",
                            ["don2"] = "2",
                            ["don3"] = "3",
                            ["rim1"] = "r",
                            ["rim2"] = "[",
                            ["rim3"] = "]"
                        }
                    }
                };
            }

            Settings settings = JsonSerializer.Deserialize<Settings>(fs);
            return settings ?? new Settings();
        }
    }
}