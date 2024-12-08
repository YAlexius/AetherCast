using Microsoft.UI.Xaml;
using System;

namespace AetherCast.Helpers
{
    /// <summary>
    /// 提供窗口相关的辅助方法，用于在 WinUI 3 应用程序中处理窗口操作
    /// </summary>
    public static class WindowHelper
    {
        /// <summary>
        /// 获取包含指定 UI 元素的窗口实例
        /// </summary>
        /// <param name="element">需要查找所属窗口的 UI 元素</param>
        /// <returns>包含该元素的窗口实例</returns>
        /// <exception cref="InvalidOperationException">
        /// 当元素未连接到窗口、找不到主窗口或元素属于其他窗口时抛出
        /// </exception>
        public static Window GetWindowForElement(UIElement element)
        {
            // 首先确保元素已连接到 XamlRoot
            if (element.XamlRoot == null)
            {
                throw new InvalidOperationException($"此元素未连接到任何窗口的视觉树中。请确保元素已被加载到界面上。");
            }

            // 从应用程序实例获取主窗口
            var app = Application.Current as App;
            if (app?.MainWindow == null)
            {
                throw new InvalidOperationException($"未找到应用程序的主窗口。请确保应用程序已正确初始化。");
            }

            var window = app.MainWindow;

            // 验证元素确实属于这个窗口
            if (window.Content?.XamlRoot != element.XamlRoot)
            {
                throw new InvalidOperationException($"此元素属于其他窗口。请确保在正确的窗口上下文中使用该元素。");
            }

            return window;
        }

        /// <summary>
        /// 尝试获取包含指定 UI 元素的窗口实例，如果无法获取则返回 null
        /// </summary>
        /// <param name="element">需要查找所属窗口的 UI 元素</param>
        /// <returns>包含该元素的窗口实例，如果无法找到则返回 null</returns>
        public static Window? TryGetWindowForElement(UIElement element)
        {
            try
            {
                return GetWindowForElement(element);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}