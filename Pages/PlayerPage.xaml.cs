using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace AetherCast.Pages
{
    // ʵ�� IDisposable �ӿ����淶��Դ����
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
                System.Diagnostics.Debug.WriteLine($"��ʼ��ý�岥����ʧ��: {ex.Message}");
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

                    // �¼��ȴ�ý�����
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
                System.Diagnostics.Debug.WriteLine($"����ý���ļ�ʧ��: {ex.Message}");
                throw;
            }
        }

        // ��׼Dispose���ԭ����Cleanup
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

        // ҳ�浼���뿪ʱ����Դ����
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Dispose();
        }
    }
}