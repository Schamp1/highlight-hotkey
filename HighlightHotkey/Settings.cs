using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HighlightHotkey
{
    class SettingsAccessor
    {
        public static Settings GetSettings()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            return config.GetRequiredSection("Settings").Get<Settings>();
        }
    }

    public sealed class Settings
    {
        public string HighlightsFolderPath { get; set; }
        public string Hotkey { get; set; }
    }
}
