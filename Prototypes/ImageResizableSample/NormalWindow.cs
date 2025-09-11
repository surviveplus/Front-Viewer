#if WINDOWS
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageResizableSample
{
    public class NormalWindow : Window
    {
        private readonly WindowManager windowManager;
        private readonly Image imageControl;
        private readonly ScrollViewer scrollViewer;
        private double zoomFactor = 1.0; // 100% zoom by default
        
        public double ZoomFactor 
        { 
            get => zoomFactor; 
            set 
            { 
                zoomFactor = Math.Max(0.1, Math.Min(10.0, value)); // Limit zoom between 10% and 1000%
                UpdateImageSize();
            } 
        }
        
        public double ScrollOffsetX => scrollViewer?.HorizontalOffset ?? 0;
        public double ScrollOffsetY => scrollViewer?.VerticalOffset ?? 0;
        public double ViewportWidth => scrollViewer?.ViewportWidth ?? Width;
        public double ViewportHeight => scrollViewer?.ViewportHeight ?? Height;
        
        public NormalWindow(WindowManager manager, BitmapImage? sampleImage)
        {
            windowManager = manager;
            
            // Window properties
            Title = "ImageResizableSample - 通常ウィンドウ";
            Width = 800;
            Height = 600;
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.CanResize;
            Background = SystemColors.ControlBrush;
            
            // Create image control
            imageControl = new Image
            {
                Stretch = Stretch.None, // Changed from Uniform to None to allow manual sizing
                Source = sampleImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            // Create scroll viewer to handle overflow
            scrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = imageControl
            };
            
            // Create main content
            var border = new Border
            {
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Child = scrollViewer
            };
            
            Content = border;
            
            // Initialize image size
            UpdateImageSize();
            
            // Event handlers
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseWheel += OnMouseWheel;
            KeyDown += OnKeyDown;
        }
        
        private void UpdateImageSize()
        {
            if (imageControl.Source is BitmapImage bitmap)
            {
                imageControl.Width = bitmap.PixelWidth * zoomFactor;
                imageControl.Height = bitmap.PixelHeight * zoomFactor;
            }
        }
        
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Zoom in/out with mouse wheel
            double zoomStep = 0.1;
            if (e.Delta > 0)
            {
                ZoomFactor += zoomStep; // Zoom in
            }
            else
            {
                ZoomFactor -= zoomStep; // Zoom out
            }
            e.Handled = true;
        }
        
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+0 to reset zoom to 100%
            if (e.Key == Key.D0 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ZoomFactor = 1.0;
                e.Handled = true;
            }
        }
        
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Switch to transparent window
            windowManager.SwitchWindowMode();
        }
    }
}
#endif