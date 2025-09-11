#if WINDOWS
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SwitchWindowStyleSample
{
    public class NormalWindow : Window
    {
        private readonly WindowManager windowManager;
        private readonly Image imageControl;
        
        public NormalWindow(WindowManager manager, BitmapImage? sampleImage)
        {
            windowManager = manager;
            
            // Window properties
            Title = "SwitchWindowStyleSample - 通常ウィンドウ";
            Width = 800;
            Height = 600;
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.CanResize;
            Background = SystemColors.ControlBrush;
            
            // Create image control
            imageControl = new Image
            {
                Stretch = Stretch.Uniform,
                Source = sampleImage,
                Margin = new Thickness(10)
            };
            
            // Create main content
            var border = new Border
            {
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Child = imageControl
            };
            
            Content = border;
            
            // Event handlers
            MouseLeftButtonDown += OnMouseLeftButtonDown;
        }
        
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Switch to transparent window
            windowManager.SwitchWindowMode();
        }
    }
}
#endif