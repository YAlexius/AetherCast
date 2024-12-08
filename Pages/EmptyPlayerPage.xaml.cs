using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AetherCast.Pages
{
    public sealed partial class EmptyPlayerPage : Page
    {
        // ����һ���¼������û�������ļ���ťʱ����
        public event EventHandler<RoutedEventArgs>? OpenFileRequested;

        public EmptyPlayerPage()
        {
            this.InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            // �����¼���֪ͨMainWindow�����ļ�������
            OpenFileRequested?.Invoke(this, e);
        }
    }
}