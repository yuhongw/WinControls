using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SwitchSetting
{
    class Helpers
    {
        internal static void ShowError(string v)
        {
            MessageBox.Show(v);
        }

        internal static string GetRootPath()
        {
            return Path.Combine(Application.StartupPath, "work");
        }

        internal static string GetOriginToolPath()
        {
            return Path.Combine(Application.StartupPath, @"Tools\1");
        }
    }
}
