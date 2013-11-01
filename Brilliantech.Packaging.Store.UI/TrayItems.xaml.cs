using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brilliantech.Packaging.Store.DLL;

namespace Brilliantech.Packaging.Store.UI
{
    /// <summary>
    /// TrayItems.xaml 的交互逻辑
    /// </summary>
    public partial class TrayItems : Window
    {
        PackageStoreService pss = null;
        public TrayItems()
        {
            InitializeComponent();
        }
        private string trayId=string.Empty;

        public TrayItems(string trayId)
        {
            InitializeComponent();
            this.trayId = trayId;
            pss = new PackageStoreService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (trayId.Length > 0)
                DGTraySinglePackageDetail.ItemsSource = pss.TraySPDetail(trayId);
        }
    }
}
