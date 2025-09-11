using System;

namespace SwitchWindowStyleSample
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
#if WINDOWS
            var app = new App();
            app.Run();
#else
            Console.WriteLine("SwitchWindowStyleSample - .NET 8");
            Console.WriteLine("このアプリケーションはWindows環境でのみ動作します。");
            Console.WriteLine("Windows環境で実行すると、通常のウィンドウと透明背景のウィンドウを");
            Console.WriteLine("クリックによって切り替えることができるサンプルが表示されます。");
            Console.WriteLine();
            Console.WriteLine("機能:");
            Console.WriteLine("- クリックで通常ウィンドウ ⇔ 透明ウィンドウの切り替え");
            Console.WriteLine("- 枠サイズを計算して画像位置を一致させる");
            Console.WriteLine("- 適切なリソース解放");
            Console.WriteLine();
            Console.WriteLine("ビルドして実行するには:");
            Console.WriteLine("1. Windowsマシンでこのプロジェクトを開く");
            Console.WriteLine("2. dotnet run または Visual Studioでデバッグ実行する");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            try
            {
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("Exiting...");
            }
#endif
        }
    }
}