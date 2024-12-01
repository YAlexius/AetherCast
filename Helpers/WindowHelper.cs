using Microsoft.UI.Xaml;
using System;

namespace AtherCast.Helpers
{
    public static class WindowHelper
    {
        public static Window GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot == null)
            {
                throw new InvalidOperationException("Element is not connected to a window");
            }

            var window = (Window)((Application.Current as App)?.MainWindow 
                ?? throw new InvalidOperationException("No main window found"));

            if (window.Content?.XamlRoot != element.XamlRoot)
            {
                throw new InvalidOperationException("Element belongs to a different window");
            }

            return window;
        }
    }
}
