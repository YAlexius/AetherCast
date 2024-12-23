using AetherCast.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace AetherCast
{
    public sealed partial class MainWindow : Window
    {
        private PlayerPage? currentPlayerPage;
        private EmptyPlayerPage? emptyPlayerPage;

        public MainWindow()
        {
            this.InitializeComponent();
            Title = "AetherCast";
            InitializeEmptyPlayerPage();
            RegisterEventHandlers();

            // 初始导航
            if (MainNav != null && HomeItem != null)
            {
                MainNav.SelectedItem = HomeItem;
            }
        }

        private void RegisterEventHandlers()
        {
            if (MainNav != null)
            {
                MainNav.SelectionChanged += MainNav_SelectionChanged;
            }

            if (OpenFileButton != null)
            {
                OpenFileButton.Click += OpenFileButton_Click;
            }

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            CleanupPlayer();
        }

        private void CleanupPlayer()
        {
            if (currentPlayerPage != null)
            {
                currentPlayerPage.Dispose();
                currentPlayerPage = null;
            }
        }
        private void InitializeEmptyPlayerPage()
        {
            emptyPlayerPage = new EmptyPlayerPage();
            // 订阅空状态页面的打开文件事件
            emptyPlayerPage.OpenFileRequested += async (s, e) => await OpenFile();
        }

        private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigateToPage(typeof(SettingsPage));
            }
            else if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag?.ToString())
                {
                    case "VideoLibrary":
                        NavigateToPage(typeof(VideoLibraryPage));
                        break;
                    case "DanmuLibrary":
                        NavigateToPage(typeof(DanmuLibraryPage));
                        break;
                    case "Playlist":
                        NavigateToPage(typeof(PlaylistPage));
                        break;
                    case "NowPlaying":
                        // 根据播放状态显示相应的页面
                        if (currentPlayerPage != null)
                        {
                            ContentFrame.Content = currentPlayerPage;
                        }
                        else
                        {
                            ContentFrame.Content = emptyPlayerPage;
                        }
                        break;
                    case "Home":
                    default:
                        if (ContentFrame != null && HomeGrid != null)
                        {
                            ContentFrame.Content = HomeGrid;
                        }
                        break;
                }
            }
        }

        private void NavigateToPage(Type pageType)
        {
            try
            {
                if (ContentFrame != null)
                {
                    ContentFrame.Navigate(pageType);
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Navigation Error", ex.Message).ConfigureAwait(false);
            }
        }

        private async Task OpenFile()
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.VideosLibrary
            };

            WinRT.Interop.InitializeWithWindow.Initialize(filePicker,
                WinRT.Interop.WindowNative.GetWindowHandle(this));

            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".mkv");
            filePicker.FileTypeFilter.Add(".avi");
            filePicker.FileTypeFilter.Add(".mp3");
            filePicker.FileTypeFilter.Add(".wav");

            try
            {
                StorageFile? file = await filePicker.PickSingleFileAsync();
                if (file != null)
                {
                    await PlayMediaFile(file);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialog("File Open Error", ex.Message);
            }
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            await OpenFile();
        }

        private async Task PlayMediaFile(StorageFile file)
        {
            try
            {
                // 创建新播放器页
                if (currentPlayerPage == null)
                {
                    currentPlayerPage = new PlayerPage();
                }

                // 导航
                if (ContentFrame != null)
                {
                    ContentFrame.Content = currentPlayerPage;
                }

                // 开始播放
                await currentPlayerPage.PlayMediaFile(file);

                // 导航中选定页面
                if (MainNav != null && NowPlayingItem != null)
                {
                    MainNav.SelectedItem = NowPlayingItem;
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialog("Playback Error", ex.Message);
            }
        }

        private async Task ShowErrorDialog(string title, string message)
        {
            try
            {
                if (Content?.XamlRoot != null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = title,
                        Content = message,
                        CloseButtonText = "OK",
                        XamlRoot = Content.XamlRoot
                    };

                    await dialog.ShowAsync();
                }
            }
            catch
            {
                // Fallback
                System.Diagnostics.Debug.WriteLine($"Error: {title} - {message}");
            }
        }
    }
}