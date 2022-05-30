using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.IO;

namespace HighlightHotkey
{
    internal class KeyCombinationListener
    {
        public static void Do(Action quit)
        {
            var hotkey = SettingsAccessor.GetSettings().Hotkey ?? "Control+Q";

            var map = new Dictionary<Combination, Action>
            {
                {
                    Combination.FromString(hotkey), () =>
                    {
                        var windowName = WindowsApi.GetActiveWindowTitle();
                        var selectedText = WindowsApi.GetSelectedText();

                        try
                        {
                            WindowsApi.SaveTextToFile(windowName, selectedText);
                            PlaySaveSound();
                        }
                        catch
                        {
                            Console.WriteLine("Error while saving file.");
                        }
                    }
                }
            };

            Console.WriteLine($"Press {hotkey} to save highlight.");

            Hook.GlobalEvents().OnCombination(map);
        }

        private static void PlaySaveSound()
        {
            var soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notification.wav");

            System.Media.SoundPlayer player = new(soundPath);
            player.Play();
        }
    }
}