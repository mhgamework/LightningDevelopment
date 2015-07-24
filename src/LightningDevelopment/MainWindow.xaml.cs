using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MHGameWork;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;
using MessageBox = System.Windows.MessageBox;

namespace LightningDevelopment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private HotkeyBinder hotkeyBinder;
        private CompositeActionsModule actionsModule;

        public MainWindow()
        {
            InitializeComponent();


            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);


            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            Deactivated += new EventHandler(MainWindow_Deactivated);


            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.WindowStyle = System.Windows.WindowStyle.None;
            ShowInTaskbar = false;
            Topmost = true;
            ResizeMode = System.Windows.ResizeMode.NoResize;

            executeSafe(() =>
                {
                    //actionsModule = ActionsModule.CreateFromProject("..\\src\\Modules\\Modules.csproj", AppDomain.CurrentDomain.BaseDirectory + "\\Modules.dll");
                    actionsModule = new CompositeActionsModule();
                    var lightningDevelopmentHandle = new LightningDevelopmentHandle(actionsModule);
                    actionsModule.Submodules.Add(ActionsModule.CreateFromDll(Configuration.Get.ModulesDllPath, lightningDevelopmentHandle));
                });
            



        }

        private void setupGlobalHotkey()
        {
            hotkeyBinder = new HotkeyBinder(new WindowInteropHelper(this).Handle);
            hotkeyBinder.unSetHotKey();
            hotkeyBinder.setHotKey(HotkeyBinder.KeyModifiers.Control | HotkeyBinder.KeyModifiers.Shift, Keys.G);
            hotkeyBinder.HotkeyFired += onGlobalHotkeyPressed;
        }


        /// <summary>
        /// Executes a piece of code safely so that exceptions are handled and displayed in the UI
        /// </summary>
        /// <param name="action"></param>
        private void executeSafe(Action action)
        {
            try
            {
                action();
                hideUIError();
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", ex.ToString());
                showUIError(ex);
                throw;
            }
        }

        private void showUIError(Exception exception)
        {
            textBox1.Background = new SolidColorBrush(Color.FromRgb(255, 100, 100));
        }

        private void hideUIError()
        {
            textBox1.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            setupGlobalHotkey();

            hidePopupWindow();

        }
        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            hidePopupWindow();
        }
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                hidePopupWindow();
        }
        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            if (actionsModule == null) return;
            var txt = textBox1.Text;

            if (!actionsModule.ContainsAction(txt))
                return;

            executeSafe(() => actionsModule.RunAction(txt));

            textBox1.Text = "";
        }
        private void onGlobalHotkeyPressed()
        {
            SetForegroundWindow(new WindowInteropHelper(this).Handle);
            Show();
            textBox1.Focus();
        }

        private void hidePopupWindow()
        {
            textBox1.Text = "";
            Hide();
        }




        #region Windows P/Invoke

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            hotkeyBinder.WndProc(hwnd, msg, wParam, lParam, ref handled);
            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        private static extern bool
            SetForegroundWindow(IntPtr hWnd);

        #endregion











    }
}
