using AetherCast.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Globalization;

namespace AetherCast
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? m_window;
        private SettingsService settingsService;
        private readonly Microsoft.Windows.ApplicationModel.Resources.ResourceLoader resourceLoader;

        public Window? MainWindow => m_window;

        public App()
        {
            this.InitializeComponent();
            settingsService = SettingsService.Instance;
            resourceLoader = new Microsoft.Windows.ApplicationModel.Resources.ResourceLoader();

            settingsService.ThemeChanged += SettingsService_ThemeChanged;
            settingsService.LanguageChanged += SettingsService_LanguageChanged;

            InitializeAppSettings();
        }

        private void InitializeAppSettings()
        {
            // 设置初始语言
            var currentLanguage = settingsService.GetCurrentLanguage();
            ApplicationLanguages.PrimaryLanguageOverride = currentLanguage;

            if (m_window != null)
            {
                UpdateAppTheme(settingsService.GetCurrentTheme());
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            UpdateAppTheme(settingsService.GetCurrentTheme());
            settingsService.ApplyTheme(m_window);
            m_window.Activate();
        }

        private void SettingsService_ThemeChanged(object? sender, ElementTheme theme)
        {
            if (m_window != null)
            {
                settingsService.ApplyTheme(m_window);
            }
        }

        private void SettingsService_LanguageChanged(object? sender, string languageCode)
        {
            ShowRestartRequiredDialog();
        }

        private void UpdateAppTheme(ElementTheme theme)
        {
            if (m_window?.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }
        }

        private async void ShowRestartRequiredDialog()
        {
            if (m_window == null) return;

            var dialog = new ContentDialog
            {
                XamlRoot = m_window.Content.XamlRoot,
                // 使用 resourceLoader 加载本地化字符串
                Title = resourceLoader.GetString("RestartRequired/Title"),
                Content = resourceLoader.GetString("RestartRequired/Content"),
                PrimaryButtonText = resourceLoader.GetString("RestartRequired/PrimaryButton"),
                CloseButtonText = resourceLoader.GetString("RestartRequired/CloseButton")
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Application.Current.Exit();
            }
        }
    }
}
