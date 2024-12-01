using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using System;
using AtherCast.Helpers;

namespace AtherCast.Pages
{
    /// <summary>
    /// Video library page for managing video content.
    /// </summary>
    public sealed partial class VideoLibraryPage : Page
    {
        public VideoLibraryPage()
        {
            this.InitializeComponent();
            InitializeVideoLibrary();
        }

        private void InitializeVideoLibrary()
        {
            if (ImportVideoButton != null)
            {
                ImportVideoButton.Click += ImportVideoButton_Click;
            }
        }

        private async void ImportVideoButton_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.VideosLibrary
            };

            // Initialize file picker with the current window handle
            var window = WindowHelper.GetWindowForElement(this);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker,
                WinRT.Interop.WindowNative.GetWindowHandle(window));

            // Add video file types
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".mkv");
            filePicker.FileTypeFilter.Add(".avi");

            try
            {
                StorageFile file = await filePicker.PickSingleFileAsync();
                if (file != null)
                {
                    // TODO: Add video to library
                    await AddVideoToLibrary(file);
                }
            }
            catch (Exception ex)
            {
                if (this.XamlRoot != null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"Failed to import video: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
        }

        private async System.Threading.Tasks.Task AddVideoToLibrary(StorageFile file)
        {
            // TODO: Implement video library management
            // This is where you would:
            // 1. Create a thumbnail
            // 2. Save video metadata
            // 3. Add to the GridView
            // For now, we'll just show a success message
            
            if (this.XamlRoot != null)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Success",
                    Content = $"Video '{file.Name}' imported successfully",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Load existing videos when navigating to this page
            LoadExistingVideos();
        }

        private void LoadExistingVideos()
        {
            // TODO: Load existing videos from storage
            // This will be implemented to load and display previously imported videos
        }
    }
}
