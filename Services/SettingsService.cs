using Microsoft.UI.Xaml;
using System;
using Windows.Globalization;
using Windows.Storage;

namespace AetherCast.Services
{
    /// <summary>
    /// 管理应用程序设置的服务类，实现为线程安全的单例模式
    /// </summary>
    public class SettingsService
    {
        private const string LanguageKey = "AppLanguage";
        private const string ThemeKey = "AppTheme";
        private string currentLanguage;
        private ElementTheme currentTheme;


        private static readonly Lazy<SettingsService> lazy =
            new Lazy<SettingsService>(() => new SettingsService());

        public static SettingsService Instance => lazy.Value;


        public event EventHandler<string>? LanguageChanged;
        public event EventHandler<ElementTheme>? ThemeChanged;

        private SettingsService()
        {
            currentLanguage = ApplicationData.Current.LocalSettings.Values[LanguageKey] as string ?? "zh-CN";
            var themeString = ApplicationData.Current.LocalSettings.Values[ThemeKey] as string ?? "System";
            currentTheme = themeString switch
            {
                "Light" => ElementTheme.Light,
                "Dark" => ElementTheme.Dark,
                _ => ElementTheme.Default
            };

            ApplicationLanguages.PrimaryLanguageOverride = currentLanguage;
        }

        public string GetCurrentLanguage() => currentLanguage;

        public ElementTheme GetCurrentTheme() => currentTheme;

        public void SetLanguage(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                throw new ArgumentNullException(nameof(languageCode));

            if (languageCode != currentLanguage)
            {
                currentLanguage = languageCode;
                ApplicationData.Current.LocalSettings.Values[LanguageKey] = languageCode;
                ApplicationLanguages.PrimaryLanguageOverride = languageCode;
                LanguageChanged?.Invoke(this, languageCode);
            }
        }

        public void SetTheme(ElementTheme theme)
        {
            if (theme != currentTheme)
            {
                currentTheme = theme;
                var themeString = theme switch
                {
                    ElementTheme.Light => "Light",
                    ElementTheme.Dark => "Dark",
                    _ => "System"
                };
                ApplicationData.Current.LocalSettings.Values[ThemeKey] = themeString;
                ThemeChanged?.Invoke(this, theme);
            }
        }

        public void ApplyTheme(Window window)
        {
            if (window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = currentTheme;
            }
        }
    }
}