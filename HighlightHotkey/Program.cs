using Microsoft.Extensions.Configuration;
using System;
using System.Windows.Forms;

namespace HighlightHotkey
{
    class InterceptKeys
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Action<Action> action = KeyCombinationListener.Do;
            action(Application.Exit);
            Application.Run(new ApplicationContext());
        }
    }
}
