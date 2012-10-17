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


            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);


            hotkeyBinder = new HotkeyBinder(new WindowInteropHelper(this).Handle);
            hotkeyBinder.unSetHotKey();
            hotkeyBinder.setHotKey(HotkeyBinder.KeyModifiers.Control | HotkeyBinder.KeyModifiers.Shift, Keys.G);
            hotkeyBinder.HotkeyFired += delegate
                                        {
                                            SetForegroundWindow(new WindowInteropHelper(this).Handle);
                                            Show();
                                            textBox1.Focus();
                                        };

            Hide();



        }

        private

        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            Hide();
        }
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                textBox1.Text = "";
                Hide();
                
            }
        }

        private HotkeyBinder hotkeyBinder;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            hotkeyBinder.WndProc(hwnd, msg, wParam, lParam, ref handled);
            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        private static extern bool
        SetForegroundWindow(IntPtr hWnd);







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

            var handle = new LightningDevelopmentHandle();

            foreach (var pluginType in listPlugins(modulesAssembly))
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType);
                plugin.Initialize(handle);
                DI.CurrentBindings.SetBinding(pluginType, plugin);
            }

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
        private IEnumerable<Type> listPlugins(Assembly ass)
        {
            return ass.GetTypes().Where(t => t.GetInterfaces().Where(i => i == typeof(IPlugin)).Count() != 0);
        }
    }
}
