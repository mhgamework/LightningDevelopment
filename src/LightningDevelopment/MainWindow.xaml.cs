using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private Dictionary<string, IQuickAction> actions = new Dictionary<string, IQuickAction>();

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            var txt = textBox1.Text;
            if (!actions.ContainsKey(txt))
                return;
            actions[txt].Execute();
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
