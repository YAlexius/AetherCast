using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace AetherCast.Pages
{
    // 实现 IDisposable 接口来规范资源清理
    public sealed partial class PlayerPage : Page, IDisposable
    {
        private MediaPlayer? mediaPlayerCore;
        private bool isDisposed;

        public PlayerPage()
        {
            this.InitializeComponent();
            InitializeMediaPlayer();
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
                    mediaPlayer.AreTransportControlsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化媒体播放器失败: {ex.Message}");
                throw;
            }
        }

        public async Task PlayMediaFile(StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            try
            {
                if (mediaPlayerCore != null && !isDisposed)
                {
                    var mediaSource = MediaSource.CreateFromStorageFile(file);
                    mediaPlayerCore.Source = mediaSource;

                    // 事件等待媒体加载
                    var tcs = new TaskCompletionSource<bool>();

                    void OnMediaOpened(MediaPlayer sender, object args)
                    {
                        mediaPlayerCore.MediaOpened -= OnMediaOpened;
                        tcs.SetResult(true);
                    }

                    mediaPlayerCore.MediaOpened += OnMediaOpened;
                    await tcs.Task;

                    mediaPlayerCore.Play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"播放媒体文件失败: {ex.Message}");
                throw;
            }
        }

        // 标准Dispose替代原来的Cleanup
        public void Dispose()
        {
            if (!isDisposed)
            {
                if (mediaPlayerCore != null)
                {
                    mediaPlayerCore.Dispose();
                    mediaPlayerCore = null;
                }
                isDisposed = true;
            }
        }

        // 页面导航离开时的资源清理
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Dispose();
        }
    }
}