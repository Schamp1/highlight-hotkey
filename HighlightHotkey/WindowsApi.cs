using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HighlightHotkey
{
    class WindowsApi
    {
        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new(nChars);
            var handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0) return Buff.ToString();

            return null;
        }

        public async static void SaveTextToFile(string windowTitle, string selectedText)
        {
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            var fileName = CleanFileName(windowTitle) + ".md";
            var filePath = Path.Combine(SettingsAccessor.GetSettings().HighlightsFolderPath ?? projectDirectory, fileName);

            if (File.Exists(filePath))
            {
                await File.AppendAllTextAsync(filePath, $"\n\n---\n\n{selectedText}");
            }
            else
            {
                await File.WriteAllTextAsync(filePath, selectedText);
            }
        }

        public static string GetSelectedText()
        {
            PressKey(Keys.ControlKey, false);
            PressKey(Keys.C, false);
            PressKey(Keys.C, true);
            PressKey(Keys.ControlKey, true);

            System.Threading.Thread.Sleep(100);

            return Clipboard.ContainsText() ? Clipboard.GetText(TextDataFormat.UnicodeText) : "";
        }

        private static void PressKey(Keys key, bool up)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            if (up)
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            else
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }

        private static string CleanFileName(string name)
        {
            name = name
            // remove file extension
                .Replace(".txt", "")
                .Replace(".pdf", "")

            // remove common window title suffix
                .Replace("[MOBI] — E-book viewer", "")
                .Replace("[EPUB] — E-book viewer", "")
                .Replace(".pdf - Adobe Acrobat Reader DC (32-bit)", "")
                .Replace(" - Brave", "")
                .Replace(" - Edge", "")
                .Replace(" - Vivaldi", "")
                .Replace(" - Google Chrome", "");

            name = Regex.Replace(name, " - Personal - Microsoft.? Edge", "");
            name = Regex.Replace(name, " and \\d\\d? more pages?", "");

            // important: make sure the file name is allowed by Windows
            name = name
                .Replace(":", " -")
                .Replace("\\", "-")
                .Replace("/", "-")
                .Replace("|", "-")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("?", "")
                .Replace("*", "")
                .Replace("\"", "")
                .Trim();

            return name.Substring(0, Math.Min(name.Length, 250));
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
    }
}
