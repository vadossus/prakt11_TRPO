using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacientApp1
{
    public static class ThemeHelper
    {
        private static readonly string[] _themePaths = {
            "Colors/WhiteTheme.xaml",
            "Colors/DarkTheme.xaml"
        };

        public static string CurrentTheme
        {
            get
            {
                string savedPath = Properties.Settings.Default.ThemePath;
                return string.IsNullOrEmpty(savedPath) ? _themePaths[0] : savedPath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value != ".")
                {
                    Properties.Settings.Default.ThemePath = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static void ApplyTheme(string themePath)
        {
            var newTheme = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            var oldTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => _themePaths.Any(path =>
                    d.Source != null && d.Source.OriginalString.EndsWith(path)));

            if (oldTheme != null)
            {
                int index = Application.Current.Resources.MergedDictionaries.IndexOf(oldTheme);
                Application.Current.Resources.MergedDictionaries[index] = newTheme;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Insert(0, newTheme);
            }

            CurrentTheme = themePath;
        }

        public static void ApplySavedTheme()
        {
            ApplyTheme(CurrentTheme);
        }

        public static void ToggleTheme()
        {
            var newTheme = CurrentTheme == _themePaths[0] ? _themePaths[1] : _themePaths[0];
            ApplyTheme(newTheme);
        }

        public static bool IsDarkTheme => CurrentTheme == _themePaths[1];
    }
}
