using System;
using System.Runtime.InteropServices;

#if WINDOWS
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AppNet8
{
    public partial class MainWindow : Window
    {
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_EXSTYLE = -20;
        private const int LWA_ALPHA = 0x2;
        private const int LWA_COLORKEY = 0x1;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        private bool isLayeredMode = true;
        private BitmapImage? sampleImage;
        private System.Windows.Controls.Image imageControl;

        public MainWindow()
        {
            InitializeComponent();
            LoadSampleImage();
            SetupLayeredWindow();
        }

        private void InitializeComponent()
        {
            // Window properties
            this.Width = 1024;
            this.Height = 1024;
            this.WindowStyle = WindowStyle.None;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "Layered Window Sample - .NET 8";
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 255)); // Magenta
            this.AllowsTransparency = true;
            
            // Create image control
            imageControl = new System.Windows.Controls.Image();
            imageControl.Stretch = Stretch.Fill;
            
            // Set content
            this.Content = imageControl;
            
            // Event handlers
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
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
                    // If sample.png doesn't exist, create a simple fallback image
                    CreateFallbackImage();
                }
                
                if (sampleImage != null)
                {
                    imageControl.Source = sampleImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"画像の読み込みに失敗しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                CreateFallbackImage();
            }
        }

        private void CreateFallbackImage()
        {
            // Create a fallback image using Drawing.Bitmap and convert to WPF BitmapSource
            var bitmap = new Bitmap(1024, 1024);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(System.Drawing.Color.Transparent);
                using (Font font = new Font("Arial", 120, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(System.Drawing.Color.Black))
                {
                    SizeF textSize = g.MeasureString("Sample", font);
                    float x = (1024 - textSize.Width) / 2;
                    float y = (1024 - textSize.Height) / 2;
                    g.DrawString("Sample", font, brush, x, y);
                }
            }
            
            // Convert System.Drawing.Bitmap to WPF BitmapSource
            sampleImage = ConvertBitmapToBitmapImage(bitmap);
            if (sampleImage != null)
            {
                imageControl.Source = sampleImage;
            }
            bitmap.Dispose();
        }
        
        private BitmapImage? ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    memoryStream.Position = 0;
                    
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    
                    return bitmapImage;
                }
            }
            catch
            {
                return null;
            }
        }

        private void SetupLayeredWindow()
        {
            var windowInteropHelper = new WindowInteropHelper(this);
            var hwnd = windowInteropHelper.Handle;
            
            if (hwnd == IntPtr.Zero)
            {
                // If handle is not created yet, wait for loaded event
                this.Loaded += (s, e) => SetupLayeredWindow();
                return;
            }

            if (isLayeredMode)
            {
                // Set layered window style
                int currentStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, currentStyle | WS_EX_LAYERED);
                
                // Make magenta color transparent
                SetLayeredWindowAttributes(hwnd, 0xFF00FF, 255, LWA_COLORKEY);
                
                this.WindowStyle = WindowStyle.None;
                this.ShowInTaskbar = false;
            }
            else
            {
                // Remove layered window style
                int currentStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, currentStyle & ~WS_EX_LAYERED);
                
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.ShowInTaskbar = true;
                this.Background = SystemColors.ControlBrush;
            }
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLayeredMode = !isLayeredMode;
            SetupLayeredWindow();
        }
    }
}
#endif

namespace AppNet8
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
#if WINDOWS
            var app = new System.Windows.Application();
            app.Run(new MainWindow());
#else
            Console.WriteLine("Layered Window Sample - .NET 8");
            Console.WriteLine("このアプリケーションはWindows環境でのみ動作します。");
            Console.WriteLine("Windows環境で実行すると、透明背景のPNG画像が表示され、");
            Console.WriteLine("クリックによってLayered Windowモードと通常ウィンドウモードを切り替えることができます。");
            Console.WriteLine();
            Console.WriteLine("ビルドして実行するには:");
            Console.WriteLine("1. Windowsマシンでこのプロジェクトを開く");
            Console.WriteLine("2. sample.pngファイルをビルド出力フォルダにコピーする");
            Console.WriteLine("3. dotnet run または Visual Studioでデバッグ実行する");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
#endif
        }
    }
}