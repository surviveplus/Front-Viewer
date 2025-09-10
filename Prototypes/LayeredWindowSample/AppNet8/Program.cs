using System;
using System.Runtime.InteropServices;

#if WINDOWS
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AppNet8
{
    public partial class MainForm : Form
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
        private Image? sampleImage;

        public MainForm()
        {
            InitializeComponent();
            LoadSampleImage();
            SetupLayeredWindow();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.ClientSize = new System.Drawing.Size(1024, 1024);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Layered Window Sample - .NET 8";
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.ResumeLayout(false);
        }

        private void LoadSampleImage()
        {
            try
            {
                string imagePath = Path.Combine(Application.StartupPath, "sample.png");
                if (File.Exists(imagePath))
                {
                    sampleImage = Image.FromFile(imagePath);
                }
                else
                {
                    // If sample.png doesn't exist, create a simple fallback image
                    CreateFallbackImage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"画像の読み込みに失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CreateFallbackImage();
            }
        }

        private void CreateFallbackImage()
        {
            sampleImage = new Bitmap(1024, 1024);
            using (Graphics g = Graphics.FromImage(sampleImage))
            {
                g.Clear(Color.Transparent);
                using (Font font = new Font("Arial", 120, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    SizeF textSize = g.MeasureString("Sample", font);
                    float x = (1024 - textSize.Width) / 2;
                    float y = (1024 - textSize.Height) / 2;
                    g.DrawString("Sample", font, brush, x, y);
                }
            }
        }

        private void SetupLayeredWindow()
        {
            if (isLayeredMode)
            {
                // Set layered window style
                int currentStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle | WS_EX_LAYERED);
                
                // Make magenta color transparent
                SetLayeredWindowAttributes(this.Handle, 0xFF00FF, 255, LWA_COLORKEY);
                
                this.FormBorderStyle = FormBorderStyle.None;
                this.ShowInTaskbar = false;
            }
            else
            {
                // Remove layered window style
                int currentStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle & ~WS_EX_LAYERED);
                
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.ShowInTaskbar = true;
                this.BackColor = SystemColors.Control;
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (sampleImage != null)
            {
                e.Graphics.DrawImage(sampleImage, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            isLayeredMode = !isLayeredMode;
            SetupLayeredWindow();
            this.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sampleImage?.Dispose();
            }
            base.Dispose(disposing);
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
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