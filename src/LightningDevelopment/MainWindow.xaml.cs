using System;
using System.Collections.Generic;
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
using SHDocVw;
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
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public MainWindow()
        {
            InitializeComponent();


            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);

            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.WindowStyle = System.Windows.WindowStyle.None;

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            ShowInTaskbar = false;
            Topmost = true;

            Deactivated += new EventHandler(MainWindow_Deactivated);
            ResizeMode = System.Windows.ResizeMode.NoResize;

            

        }

        

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                buildModules();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            //actions["test"].Execute();

            var t = new Thread(fetchExplorerInfo);
            t.Start();

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);


            //put this code in the onload method of your form
            unSetHotKey();
            setHotKey(KeyModifiers.Control | KeyModifiers.Shift, Keys.G);
            //and set up a form closed event and call

            Hide();
            
            

        }

        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            Hide();
        }
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Hide();
        }
        private void fetchExplorerInfo()
        {
            var windows = new ShellWindows();
            while (true)
            {
                try
                {
                    var foreground = GetForegroundWindow();
                    foreach (InternetExplorer item in windows)
                    {
                        if (item.HWND != foreground.ToInt32()) continue;
                        Context.WorkingDir =  item.LocationURL.Replace("file:///","");
                        Context.ActiveFile = item.Document.FocusedItem.Path;
                    }
                }
                catch (Exception)
                {
                }


                System.Threading.Thread.Sleep(200);
            }
        }
        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)lParam >> 16) & 0xFFFF);
                    KeyModifiers modifier = (KeyModifiers)((int)lParam & 0xFFFF);
                    //put your on hotkey code here
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());
                    //end hotkey code
                    SetForegroundWindow(new WindowInteropHelper(this).Handle);
                    Show();
                    textBox1.Focus();
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        private static extern bool
        SetForegroundWindow(IntPtr hWnd);

        //API Imports
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd, // handle to window    
            int id, // hot key identifier    
            KeyModifiers fsModifiers, // key-modifier options    
            Keys vk    // virtual-key code    
            );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd, // handle to window    
            int id      // hot key identifier    
            );
        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance
        public enum KeyModifiers        //enum to call 3rd parameter of RegisterHotKey easily
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        public bool setHotKey(KeyModifiers Kmds, Keys key)
        {
            return RegisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_ID, Kmds, key);
        }
        public bool unSetHotKey()
        {
            return UnregisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_ID);
        }
        const int WM_HOTKEY = 0x0312;//magic hotkey message identifier



        private Dictionary<string, IQuickAction> actions = new Dictionary<string, IQuickAction>();

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            var txt = textBox1.Text;
            if (!actions.ContainsKey(txt))
                return;
            actions[txt].Execute();
            textBox1.Text = "";
        }

        private void buildModules()
        {
            var engine = new Microsoft.Build.Evaluation.Project("..\\src\\Modules\\Modules.csproj");
            engine.Build();

            var modulesAssembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "\\Modules.dll");

            var actionTypes = listQuickActions(modulesAssembly);

            foreach (var type in actionTypes)
            {
                var obj = (IQuickAction)Activator.CreateInstance(type);
                actions.Add(obj.Command, obj);
            }

        }

        private IEnumerable<Type> listQuickActions(Assembly ass)
        {
            return ass.GetTypes().Where(t => t.GetInterfaces().Where(i => i == typeof(IQuickAction)).Count() != 0);
        }
    }
}
