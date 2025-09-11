#if WINDOWS
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageResizableSample
{
    public class WindowManager
    {
        private NormalWindow? normalWindow;
        private TransparentWindow? transparentWindow;
        private BitmapImage? sampleImage;
        private bool isTransparentMode = false;
        
        public WindowManager()
        {
            LoadSampleImage();
        }
        
        private void LoadSampleImage()
        {
            try
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.png");
                if (File.Exists(imagePath))
                {
                    sampleImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                }
                else
                {
                    MessageBox.Show("sample.png ファイルが見つかりません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"画像の読み込みに失敗しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public void ShowNormalWindow()
        {
            if (normalWindow == null)
            {
                normalWindow = new NormalWindow(this, sampleImage);
                normalWindow.Closed += OnWindowClosed;
            }
            
            HideTransparentWindow();
            PositionWindow(normalWindow, false);
            normalWindow.Show();
            isTransparentMode = false;
        }
        
        public void ShowTransparentWindow()
        {
            if (transparentWindow == null)
            {
                transparentWindow = new TransparentWindow(this, sampleImage);
                transparentWindow.Closed += OnWindowClosed;
            }
            
            HideNormalWindow();
            PositionWindow(transparentWindow, true);
            transparentWindow.Show();
            isTransparentMode = true;
        }
        
        public void SwitchWindowMode()
        {
            if (isTransparentMode)
            {
                ShowNormalWindow();
            }
            else
            {
                ShowTransparentWindow();
            }
        }
        
        private void PositionWindow(Window window, bool isTransparent)
        {
            if (isTransparent && normalWindow != null )
            {
                // Transparent window should be positioned to align content with normal window
                // Account for the frame size of the normal window
                var frameThickness = SystemParameters.WindowNonClientFrameThickness;
                var borderThickness = SystemParameters.ResizeFrameVerticalBorderWidth;
                var captionHeight = SystemParameters.WindowCaptionHeight;
                
                window.Left = normalWindow.Left + frameThickness.Left;
                window.Top = normalWindow.Top + frameThickness.Top + captionHeight;
                window.Width = normalWindow.Width - frameThickness.Left - frameThickness.Right;
                window.Height = normalWindow.Height - frameThickness.Top - frameThickness.Bottom - captionHeight;
            }
            else if (!isTransparent && transparentWindow != null )
            {
                // Normal window should be positioned to align content with transparent window
                var frameThickness = SystemParameters.WindowNonClientFrameThickness;
                var borderThickness = SystemParameters.ResizeFrameVerticalBorderWidth;
                var captionHeight = SystemParameters.WindowCaptionHeight;
                
                window.Left = transparentWindow.Left - frameThickness.Left;
                window.Top = transparentWindow.Top - frameThickness.Top - captionHeight;
                window.Width = transparentWindow.Width + frameThickness.Left + frameThickness.Right;
                window.Height = transparentWindow.Height + frameThickness.Top + frameThickness.Bottom + captionHeight;
            }
            else
            {
                // Default positioning - center screen
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Width = 800;
                window.Height = 600;
            }
        }
        
        private void HideNormalWindow()
        {
            if (normalWindow != null && normalWindow.IsVisible)
            {
                normalWindow.Hide();
            }
        }
        
        private void HideTransparentWindow()
        {
            if (transparentWindow != null && transparentWindow.IsVisible)
            {
                transparentWindow.Hide();
            }
        }
        
        private void OnWindowClosed(object? sender, EventArgs e)
        {
            // When one window is closed, close the other and cleanup
            CleanupAndShutdown();
        }
        
        private void CleanupAndShutdown()
        {
            try
            {
                if (normalWindow != null)
                {
                    normalWindow.Closed -= OnWindowClosed;
                    if (normalWindow.IsVisible)
                        normalWindow.Close();
                    normalWindow = null;
                }
                
                if (transparentWindow != null)
                {
                    transparentWindow.Closed -= OnWindowClosed;
                    if (transparentWindow.IsVisible)
                        transparentWindow.Close();
                    transparentWindow = null;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't prevent shutdown
                System.Diagnostics.Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }
    }
}
#endif