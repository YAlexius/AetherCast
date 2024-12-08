using AetherCast.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace AetherCast.Pages
{
    public sealed partial class SettingsPage : Page
    {
        private readonly SettingsService settingsService;

        public SettingsPage()
        {
            this.InitializeComponent();
            settingsService = SettingsService.Instance;
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            // 加载当前语言设置
            var currentLanguage = settingsService.GetCurrentLanguage();
            LanguageComboBox.SelectedItem = LanguageComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == currentLanguage);

            // 加载当前主题设置
            var currentTheme = settingsService.GetCurrentTheme();
            var themeResourceKey = GetThemeResourceKey(currentTheme);
            ThemeComboBox.SelectedItem = ThemeComboBox.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Name == themeResourceKey);
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only proceed if we have a valid selection
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem &&
                selectedItem.Tag?.ToString() is string languageCode &&
                languageCode != settingsService.GetCurrentLanguage())
            {
                // Update the language setting
                settingsService.SetLanguage(languageCode);
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var theme = GetThemeFromResourceKey(selectedItem.Name);
                settingsService.SetTheme(theme);
            }
        }

        private string GetThemeResourceKey(ElementTheme theme)
        {
            return theme switch
            {
                ElementTheme.Light => "LightThemeItem",
                ElementTheme.Dark => "DarkThemeItem",
                _ => "SystemThemeItem"
            };
        }

        private ElementTheme GetThemeFromResourceKey(string resourceKey)
        {
            return resourceKey switch
            {
                "LightThemeItem" => ElementTheme.Light,
                "DarkThemeItem" => ElementTheme.Dark,
                "SystemThemeItem" => ElementTheme.Default,
                _ => ElementTheme.Default
            };
        }
    }
}
