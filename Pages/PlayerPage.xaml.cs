using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace AetherCast.Pages
{
    public sealed partial class PlayerPage : Page
    {
        private MediaPlayer? mediaPlayerCore;

        public PlayerPage()
        {
            this.InitializeComponent();
            InitializeMediaPlayer();
        }

        private void InitializeMediaPlayer()
        {
            mediaPlayerCore = new MediaPlayer();
            if (mediaPlayer != null)
            {
                mediaPlayer.SetMediaPlayer(mediaPlayerCore);
                mediaPlayerCore.AutoPlay = true;
            }
        }

        public async Task PlayMediaFile(StorageFile file)
        {
            try
            {
                if (mediaPlayerCore != null)
                {
                    var mediaSource = MediaSource.CreateFromStorageFile(file);
                    mediaPlayerCore.Source = mediaSource;
                    await Task.Delay(100); // Small delay to ensure media is loaded
                    mediaPlayerCore.Play();
                }
            }
            catch (Exception ex)
            {
                // Handle error
                throw;
            }
        }

        public void Cleanup()
        {
            if (mediaPlayerCore != null)
            {
                mediaPlayerCore.Dispose();
                mediaPlayerCore = null;
            }
        }
    }
}