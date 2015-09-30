using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UriInternal
{
    /// <summary>
    /// Interaction logic for DlgEnum.xaml
    /// </summary>
    public partial class DlgResource : Window
    {
        public DlgResource()
        {
            InitializeComponent();
            Model = DataContext as DlgResourceModel;
        }

        public DlgResourceModel Model { get; set; }

    }
}
