using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AetherCast.Pages
{
    public sealed partial class EmptyPlayerPage : Page
    {
        // 定义一个事件，当用户点击打开文件按钮时触发
        public event EventHandler<RoutedEventArgs>? OpenFileRequested;

        public EmptyPlayerPage()
        {
            this.InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            // 触发事件，通知MainWindow处理文件打开请求
            OpenFileRequested?.Invoke(this, e);
        }
    }
}