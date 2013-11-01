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
using Brilliantech.Framework;
using Brilliantech.Packaging.Store.DLL;
using Brilliantech.Packaging.Store.Data.Enum;
using System.Collections;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Framework.Messages;
using Xceed.Wpf.Toolkit;
using Microsoft.Win32;

namespace Brilliantech.Packaging.Store.UI
{
    /// <summary>
    /// TraySearch.xaml 的交互逻辑
    /// </summary>
    public partial class TraySearch : Window
    {
        PackageStoreService packageStoreService = null;
        IExportService exportService = null;
        public TraySearch()
        {
            InitializeComponent();
            packageStoreService = new PackageStoreService();
            exportService = new ExportService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindCondition();
        }

        private void BindCondition()
        {
            ConfigUtil config = new ConfigUtil("STORE", "config.ini");
            TBWarehouse.Text = config.Get("WAREHOUSE");
           // TBPosition.Text = config.Get("POSITION");
            ConditionService cs = new ConditionService();
            CBTrayStatus.ItemsSource = cs.GetEnumItemOptions<TrayStatus>(TrayStatus.Cancled);
            CBTrayStatus.SelectedIndex = 0;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Hashtable condi = new Hashtable();
            condi["trayId"] = TBTrayId.Text;
            condi["pid"] = TBPackageId.Text;
            condi["pnr"] = TBPartNr.Text;
            condi["wh"] = TBWarehouse.Text;
            condi["posi"] = TBPosition.Text;
            condi["status"] = CBTrayStatus.SelectedValue;
            condi["startDate"] = (DPStartDate.Text == null || DPStartDate.Text.Length==0) ? "1753/01/01" : DPStartDate.Text;
            condi["endDate"] = (DPEndDate.Text == null || DPEndDate.Text.Length == 0) ? "9999/12/31" : DPEndDate.Text;
            DGTrayItemsDetail.ItemsSource = packageStoreService.Search(condi);
            BtnExport.IsEnabled = CBTrayStatus.SelectedIndex > 0;
        }

        private void ImgCleanTB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image i = sender as Image;
            object obj = this.FindName(i.Name.Replace("CleanImg", ""));
            if (obj.GetType() == typeof(TextBox))
                (obj as TextBox).Text = string.Empty;
            else
                if (obj.GetType() == typeof(DateTimePicker))
                    (obj as DateTimePicker).Text = string.Empty;
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            if (DGTrayItemsDetail.Items.Count > 0 && DGTrayItemsDetail.SelectedItems.Count > 0)
            {
              List<string> cancleIds= DGTrayItemsDetail.SelectedCells.Where(i=>((Trays)i.Item).status!=(int)TrayStatus.Cancled).Select(i => ((Trays)i.Item).trayId).ToList<string>().ToList();
              if (cancleIds.Count > 0)
              {
                  ProcessMsg msg = packageStoreService.CancleStored(cancleIds);
                  if (msg.result)
                  {
                      new InfoBoard(MsgLevel.Successful, "取消成功！",10000).ShowDialog();
                      BtnSearch_Click(sender, e);
                  }
                  else
                  {
                      new InfoBoard(MsgLevel.Warning, msg.GetAllLevelMsgs()).ShowDialog();
                  }
              }
              else
              {
                  new InfoBoard(MsgLevel.Warning, "请选择未取消托盘").ShowDialog();
              }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {
            if(DGTrayItemsDetail.SelectedItems.Count ==1)
            {
                (new TrayItems((DGTrayItemsDetail.SelectedItem as Trays).trayId)).ShowDialog();
            }
        }

        private void BtnReprint_Click(object sender, RoutedEventArgs e)
        {
            if (DGTrayItemsDetail.SelectedItems.Count == 1)
            {
                this.PrintTrayLabel((DGTrayItemsDetail.SelectedItem as Trays).trayId);
            }
        }
        private void PrintTrayLabel(string trayId)
        {
            Message pmsg = Printer.PrintTrayLabel(trayId);
            if (pmsg.Result)
                new InfoBoard(MsgLevel.Successful, pmsg.Content).ShowDialog();
            else
                new InfoBoard(MsgLevel.Mistake, pmsg.Content).ShowDialog();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV 文件格式|*.csv";
            sfd.Title = "导出CSV文件";
            
            sfd.FileName="CSVData"+DateTime.Now.ToString("yyyyMMddHHmmss")+".csv";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                if (DGTrayItemsDetail.Items.Count > 0)
                {
                    List<string> exportIds = new List<string>();
                    foreach (Trays t in DGTrayItemsDetail.Items)
                    {
                        exportIds.Add(t.trayId);
                    }
                    ConfigUtil config = new ConfigUtil("CSVFILEDS", "csv.ini");
                    List<string> csvFields=new List<string> ();
                   string[] keys =config.GetAllNodeKey();
                   foreach (string key in keys)
                   {
                       if (config.Get(key) == "1")
                           csvFields.Add(key);
                   }
                    ProcessMsg msg = exportService.ExportTraySumPartCSV(exportIds,sfd.FileName,csvFields);
                    new InfoBoard(MsgLevel.Info, msg.GetAllLevelMsgs()).ShowDialog();
                    config = new ConfigUtil("AUTO", "config.ini");

                    if (msg.result && bool.Parse(config.Get("EXPORTLOAD")))
                        BtnSearch_Click(sender, e);
                }
            }
        }

        private void CBTrayStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnExport.IsEnabled = false;
        }
    }
}
