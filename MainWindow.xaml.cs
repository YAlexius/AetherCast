using AtherCast.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AtherCast
{
    /// <summary>
    /// Main window class that handles core application functionality and navigation
    /// </summary>
    public sealed partial class MainWindow : Microsoft.UI.Xaml.Window
    {
        private MediaPlayer? mediaPlayerCore;
        private bool isDisposed;

        public MainWindow()
        {
            this.InitializeComponent();
            Title = "AtherCast";

            InitializeMediaPlayer();
            RegisterEventHandlers();

            // Set initial navigation
            if (MainNav != null && HomeItem != null)
            {
                MainNav.SelectedItem = HomeItem;
            }
        }

        private void InitializeMediaPlayer()
        {
            try
            {
                mediaPlayerCore = new MediaPlayer();
                if (mediaPlayer != null)
                {
                    mediaPlayer.SetMediaPlayer(mediaPlayerCore);
                    mediaPlayerCore.AutoPlay = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Media Player Initialization Error", ex.Message).ConfigureAwait(false);
            }
        }

        private void RegisterEventHandlers()
        {
            if (mediaPlayerCore?.PlaybackSession != null)
            {
                mediaPlayerCore.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
            }

            if (MainNav != null)
            {
                MainNav.SelectionChanged += MainNav_SelectionChanged;
                MainNav.PaneClosing += MainNav_PaneClosing;
                MainNav.PaneOpening += MainNav_PaneOpening;
            }

            if (OpenFileButton != null)
            {
                OpenFileButton.Click += OpenFileButton_Click;
            }

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            CleanupMediaPlayer();
        }

        private void CleanupMediaPlayer()
        {
            if (!isDisposed && mediaPlayerCore != null)
            {
                if (mediaPlayerCore.PlaybackSession != null)
                {
                    mediaPlayerCore.PlaybackSession.PlaybackStateChanged -= PlaybackSession_PlaybackStateChanged;
                }

                mediaPlayerCore.Dispose();
                mediaPlayerCore = null;
                isDisposed = true;
            }
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
                        ShowPlayerView();
                        break;
                    case "Home":
                    default:
                        if (ContentFrame != null && HomeGrid != null)
                        {
                            ContentFrame.Content = HomeGrid;
                            ShowHomeView();
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
                    ShowHomeView();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Navigation Error", ex.Message).ConfigureAwait(false);
            }
        }

        private void ShowPlayerView()
        {
            if (mediaPlayer != null && HomeContent != null)
            {
                mediaPlayer.Visibility = Visibility.Visible;
                HomeContent.Visibility = Visibility.Collapsed;
                UpdateMediaPlayerLayout();
            }
        }

        private void ShowHomeView()
        {
            if (mediaPlayer != null && HomeContent != null)
            {
                mediaPlayer.Visibility = Visibility.Collapsed;
                HomeContent.Visibility = Visibility.Visible;
            }
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
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

        private async Task PlayMediaFile(StorageFile file)
        {
            try
            {
                if (mediaPlayerCore == null)
                {
                    InitializeMediaPlayer();
                }

                if (mediaPlayerCore != null)
                {
                    var mediaSource = MediaSource.CreateFromStorageFile(file);
                    mediaPlayerCore.Source = mediaSource;
                    await Task.Delay(100); // Small delay to ensure media is loaded
                    mediaPlayerCore.Play();

                    if (MainNav != null && NowPlayingItem != null)
                    {
                        MainNav.SelectedItem = NowPlayingItem;
                    }
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
                // Fallback error handling if dialog fails
                System.Diagnostics.Debug.WriteLine($"Error: {title} - {message}");
            }
        }

        private void PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
        {
            if (sender == null) return;

            DispatcherQueue?.TryEnqueue(() =>
            {
                if (mediaPlayerCore?.PlaybackSession != null)
                {
                    switch (sender.PlaybackState)
                    {
                        case MediaPlaybackState.Playing:
                        case MediaPlaybackState.Paused:
                        case MediaPlaybackState.None:
                            UpdateMediaPlayerLayout();
                            break;
                    }
                }
            });
        }

        private void MainNav_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
            UpdateMediaPlayerLayout();
        }

        private void MainNav_PaneOpening(NavigationView sender, object args)
        {
            UpdateMediaPlayerLayout();
        }

        private void UpdateMediaPlayerLayout()
        {
            if (mediaPlayer == null || MainNav == null) return;

            mediaPlayer.Margin = MainNav.IsPaneOpen
                ? new Thickness(0)
                : new Thickness(48, 0, 0, 0);
        }
    }
}
