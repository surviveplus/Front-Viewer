#if WINDOWS
using System;
using System.Windows;

namespace SwitchWindowStyleSample
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var windowManager = new WindowManager();
            windowManager.ShowNormalWindow();
        }
    }
}
#endif