#if WINDOWS
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageResizableSample
{
    public class TransparentWindow : Window
    {
        private readonly WindowManager windowManager;
        private readonly Image imageControl;
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
        
        public TransparentWindow(WindowManager manager, BitmapImage? sampleImage)
        {
            windowManager = manager;
            
            // Window properties for transparent window
            Title = "ImageResizableSample - 透明ウィンドウ";
            Width = 800;
            Height = 600;
            WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            ShowInTaskbar = true;
            Topmost = true;
            
            // Create image control
            imageControl = new Image
            {
                Stretch = Stretch.None, // Changed from Uniform to None to allow manual sizing
                Source = sampleImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            Content = imageControl;
            
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