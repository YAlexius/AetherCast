using AetherCast.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;

namespace AetherCast.Pages
{
    public sealed partial class DanmuLibraryPage : Page
    {
        public DanmuLibraryPage()
        {
            this.InitializeComponent();
            InitializePage();
        }

        private void InitializePage()
        {
            OpenFolderButton.Click += OpenFolderButton_Click;
        }

        private async void OpenFolderButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // �����ļ���ѡ����
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            // ���ͨ���������ѡ���κ��ļ���
            folderPicker.FileTypeFilter.Add("*");

            try
            {
                // ʹ�ð������ȡ��ǰ����
                var window = WindowHelper.GetWindowForElement(this);

                // ��ʼ���ļ�ѡ����
                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker,
                    WinRT.Interop.WindowNative.GetWindowHandle(window));

                // ��ʾ�ļ���ѡ����
                var folder = await folderPicker.PickSingleFolderAsync();

                if (folder != null)
                {
                    // TODO: ���������Ӵ���ѡ���ļ��е��߼�
                    // ���磺ɨ���ļ����еĵ�Ļ�ļ�
                    // await ScanDanmuFilesInFolder(folder);
                }
            }
            catch (Exception ex)
            {
                // ��ʾ����Ի���
                ContentDialog dialog = new ContentDialog
                {
                    Title = "����",
                    Content = $"�޷����ļ���: {ex.Message}",
                    CloseButtonText = "ȷ��",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }
    }
}