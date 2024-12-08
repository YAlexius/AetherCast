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
            // 创建文件夹选择器
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            // 添加通配符以允许选择任何文件夹
            folderPicker.FileTypeFilter.Add("*");

            try
            {
                // 使用帮助类获取当前窗口
                var window = WindowHelper.GetWindowForElement(this);

                // 初始化文件选择器
                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker,
                    WinRT.Interop.WindowNative.GetWindowHandle(window));

                // 显示文件夹选择器
                var folder = await folderPicker.PickSingleFolderAsync();

                if (folder != null)
                {
                    // TODO: 这里可以添加处理选中文件夹的逻辑
                    // 例如：扫描文件夹中的弹幕文件
                    // await ScanDanmuFilesInFolder(folder);
                }
            }
            catch (Exception ex)
            {
                // 显示错误对话框
                ContentDialog dialog = new ContentDialog
                {
                    Title = "错误",
                    Content = $"无法打开文件夹: {ex.Message}",
                    CloseButtonText = "确定",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }
    }
}