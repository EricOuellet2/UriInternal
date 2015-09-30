using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibWithRes;
using Microsoft.Win32;

namespace UriInternal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Model = DataContext as MainWindowModel;
        }

        public MainWindowModel Model { get; private set; }

        private void ButtonUpdateOnClick(object sender, RoutedEventArgs e)
        {
            Model.RefreshFields();
        }

        private void LoadDllAndShowResourcesOnClick(object sender, RoutedEventArgs e)
        {
            // Thanks to Thomas Levesque : http://stackoverflow.com/questions/2517407/enumerating-net-assembly-resources-at-runtime

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog(this) == true)
            {
                string path = dlg.FileName;

                Assembly asm = null;
                try
                {
                    asm = Assembly.LoadFrom(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load assembly, exception: " + ex.ToString());
                    return;
                }

                // Assembly asm = AppDomain.CurrentDomain.Load(path);

                // AppDomain.CurrentDomain.CreateInstanceAndUnwrap(path)

                DlgResource dlgResource = new DlgResource();
                dlgResource.Model.Assembly = asm;
                dlgResource.Show();
            }
        }

        private void TestResourceHelperLoadRes_OnClick(object sender, RoutedEventArgs e)
        {
            var asm = Assembly.GetAssembly(typeof(Test));

            Debug.Assert(null != ResourceHelper.LoadResourceFromUri(new Uri("pack://application:,,,/LibWithRes;component/Resource/AnyDivision/isabel-lucas.jpg")));
            Debug.Assert(null != ResourceHelper.LoadResourceFromUri(new Uri("pack://application:,,,/LibWithRes;component/Resource/AnyDivision/isabel-lucas.jpg"), asm));
            Debug.Assert(null != ResourceHelper.LoadResourceFromUri(new Uri("pack://application:,,,/Resource/AnyDivision/isabel-lucas.jpg"), asm));
            Debug.Assert(null != ResourceHelper.LoadResourceFromUri(new Uri("pack://application:,,,/Resource/Nicky Whelan.jpg")));
        }
    }
}
