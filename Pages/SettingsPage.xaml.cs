using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace AtherCast.Pages
{
    public sealed partial class SettingsPage : Page
    {
        private const string LanguageKey = "AppLanguage";
        private const string ThemeKey = "AppTheme";

        public SettingsPage()
        {
            this.InitializeComponent();
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            // Load current language setting
            var currentLanguage = Windows.Storage.ApplicationData.Current.LocalSettings.Values[LanguageKey] as string ?? "zh-CN";
            LanguageComboBox.SelectedItem = LanguageComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == currentLanguage);

            // Load current theme setting
            var currentTheme = Windows.Storage.ApplicationData.Current.LocalSettings.Values[ThemeKey] as string ?? "System";
            ThemeComboBox.SelectedItem = ThemeComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == GetThemeDisplayName(currentTheme));
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values[LanguageKey] = selectedItem.Tag.ToString();
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string theme = GetThemeValue(selectedItem.Content.ToString());
                Windows.Storage.ApplicationData.Current.LocalSettings.Values[ThemeKey] = theme;
            }
        }

        private string GetThemeValue(string? displayName)
        {
            return (displayName ?? "System") switch
            {
                "浅色" => "Light",
                "深色" => "Dark",
                _ => "System"
            };
        }

        private string GetThemeDisplayName(string theme)
        {
            return theme switch
            {
                "Light" => "浅色",
                "Dark" => "深色",
                _ => "跟随系统"
            };
        }
    }
}
