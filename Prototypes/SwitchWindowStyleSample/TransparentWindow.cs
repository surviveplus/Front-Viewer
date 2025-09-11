#if WINDOWS
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SwitchWindowStyleSample
{
    public class TransparentWindow : Window
    {
        private readonly WindowManager windowManager;
        private readonly Image imageControl;
        
        public TransparentWindow(WindowManager manager, BitmapImage? sampleImage)
        {
            windowManager = manager;
            
            // Window properties for transparent window
            Title = "SwitchWindowStyleSample - 透明ウィンドウ";
            Width = 800;
            Height = 600;
            WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            ShowInTaskbar = false;
            Topmost = true;
            
            // Create image control
            imageControl = new Image
            {
                Stretch = Stretch.Uniform,
                Source = sampleImage
            };
            
            Content = imageControl;
            
            // Event handlers
            MouseLeftButtonDown += OnMouseLeftButtonDown;
        }
        
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Allow dragging the transparent window
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch
                {
                    // Ignore drag move errors - this might happen if the window is in the middle of switching
                }
            }
            
            // Switch to normal window on double click to avoid accidental switching during drag
            if (e.ClickCount == 2)
            {
                windowManager.SwitchWindowMode();
            }
        }
    }
}
#endif