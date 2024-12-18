using System.Windows;

namespace PiggsCare.UI.Themes
{
    public static class ThemesController
    {
        public static ThemeTypes CurrentTheme { get; set; }
        public static ResourceDictionary ThemeDictionary
        {
            get => Application.Current.Resources.MergedDictionaries[0];
            set => Application.Current.Resources.MergedDictionaries[0] = value;
        }

        private static void ChangeTheme( Uri uri )
        {
            ThemeDictionary = new ResourceDictionary { Source = uri };
        }

        public static void SetTheme( ThemeTypes theme )
        {
            string themeName = null!;
            CurrentTheme = theme;

            switch (theme)
            {
                case ThemeTypes.Light: themeName = "Light"; break;
                case ThemeTypes.Dark: themeName = "Dark"; break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                {
                    ChangeTheme(new Uri($"Themes/{themeName}.xaml", UriKind.Relative));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
