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
using System.Drawing.Printing;
using Brilliantech.Framework;
using System.Collections;

namespace Brilliantech.Packaging.Store.UI
{
    /// <summary>
    /// SetPrinter.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hashtable ht = PrinterUtil.GetPrinterConfig();
            foreach (string printer in PrinterSettings.InstalledPrinters)
                CBPrinters.Items.Add(printer);
            CBPrinters.SelectedIndex = 0;
            for (int i = 0; i < CBPrinters.Items.Count; i++)
            {
                if (CBPrinters.Items[i].Equals(ht["PrinterName"].ToString()))
                {
                    CBPrinters.SelectedIndex = i;
                    break;
                }
            }
            this.TBSecond.Text = (int.Parse(ht["AutoClose"].ToString()) / 1000).ToString();
            this.TBCopy.Text = ht["Copy"].ToString();
            ConfigUtil config = new ConfigUtil("STORE", "config.ini");
            TBWarehouse.Text = config.Get("WAREHOUSE");
            TBPosition.Text = config.Get("POSITION");
            config = new ConfigUtil("DATEFORMAT", "config.ini");
            TBDateFormat.Text = config.Get("DATEFORMAT");
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            PrinterUtil.SetPrinterName(this.CBPrinters.SelectedItem.ToString());
            string sec = TBSecond.Text;
            int i = 3000;
            if (int.TryParse(sec, out i) && i > 0)
                PrinterUtil.SetPrinterAutoInterVal(i * 1000);
            int copy = 1;
            if(int.TryParse(TBCopy.Text,out copy)&& copy>0)
                PrinterUtil.SetPrintCopy(copy);
           // PrinterUtil.Save();

            ConfigUtil config = new ConfigUtil("STORE", "config.ini");
            
            config.Set("WAREHOUSE", TBWarehouse.Text);
            config.Set("POSITION", TBPosition.Text);
            config.Save();
            config = new ConfigUtil("DATEFORMAT", "config.ini");
            config.Set("DATEFORMAT", TBDateFormat.Text);
            config.Save();
            new InfoBoard(MsgLevel.Successful, "参数设置成功").ShowDialog(); 
            this.Close();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
