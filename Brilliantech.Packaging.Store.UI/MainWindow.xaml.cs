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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Brilliantech.Framework;
using Brilliantech.Packaging.Store.DLL;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Framework.Messages;

namespace Brilliantech.Packaging.Store.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        IPackageStoreService packageStoreService = null;
        IConditionService conditionService = null;
        public MainWindow()
        {
            InitializeComponent();
            packageStoreService = new PackageStoreService();
            conditionService = new ConditionService();
        }
        private List<SinglePackage> SPDataSource = null;
        private void ImgCleanTB_MouseUp(object sender, MouseEventArgs e)
        {
            Image i = sender as Image;
            TextBox tb = this.FindName(i.Name.Replace("CleanImg", "")) as TextBox;
            tb.Text = string.Empty;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void BtnSetPrinter_Click(object sender, RoutedEventArgs e)
        {
            (new Setting()).ShowDialog();
            LoadWarehouseConfig();
        }

        private void TBPackageId_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                validateTBPackageID();
            }
        }

        private void validateTBPackageID() {
            if (TBPackageId.Text.Length > 0)
            {
                ValidateMsg<SinglePackage> msg = packageStoreService.SingleCheckStore(TBPackageId.Text);
                if (!msg.Valid)
                {
                    new InfoBoard(MsgLevel.Mistake, msg.ToString()).ShowDialog();
                }
                else
                {
                    bool scand = false;
                    if (DGTrayItemsDetail.ItemsSource != null)
                        foreach (var i in DGTrayItemsDetail.ItemsSource)
                        {
                            if ((i as SinglePackage).packageID.Equals(msg.Target.packageID))
                            {
                                new InfoBoard(MsgLevel.Mistake, TBPackageId.Text + " 已经扫入！").ShowDialog();
                                scand = true;
                                break;
                            }
                        }
                    if (!scand)
                    {
                        if (SPDataSource == null) SPDataSource = new List<SinglePackage>();
                        SPDataSource.Insert(0, msg.Target);
                        DGTrayItemsDetail.ItemsSource = null;
                        DGTrayItemsDetail.ItemsSource = SPDataSource;
                        LabPackNum.Content = SPDataSource.Count;
                        //  BtnNew.IsEnabled = false;
                        BtnFinsh.IsEnabled = true;
                    }
                }
                TBPackageId.Text = string.Empty;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          //  LoadWarehouseConfig();
        }

        private void BtnFinsh_Click(object sender, RoutedEventArgs e)
        {
            if (TBWarehouse.Text.Length > 0 && TBPosition.Text.Length > 0)
            {
                if (int.Parse(LabPackNum.Content.ToString()) > 0)
                {
                    if (SPDataSource != null)
                    {
                      ProcessMsg msg= packageStoreService.CompleteStore(SPDataSource.Select(t => t.packageID).ToList<string>(), TBWarehouse.Text, TBPosition.Text.ToString());
                      if (msg.result)
                      {
                          TBTrayId.Text = msg.GetAllLevelMsgs().Trim();
                          TBPackageId.IsEnabled = false;
                          BtnFinsh.IsEnabled = false;
                          BtnRePrint.IsEnabled = true;
                          BtnNew.IsEnabled = true;
                          bool? print=new InfoBoard(MsgLevel.Successful, "入库成功，是否打印包装箱标签？\n标签号为：" + TBTrayId.Text).ShowDialog();
                          if ((bool)print)
                          {
                              this.PrintTrayLabel(TBTrayId.Text);
                          }
                      }
                      else {
                          new InfoBoard(MsgLevel.Warning, msg.GetAllLevelMsgs()).ShowDialog();
                      }
                    }
                }
                else {
                    new InfoBoard(MsgLevel.Warning, "未开始入库！").ShowDialog();
                }
            }
            else 
            {
                new InfoBoard(MsgLevel.Warning,  "未输入仓库或库位！").ShowDialog();
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            ConfigUtil config = new ConfigUtil("STORE", "config.ini");
            ProcessMsg msg = conditionService.GenPosition(config.Get("POSITION"));
            if (msg.result)
            {
                SPDataSource = null;
                DGTrayItemsDetail.ItemsSource = null;
                BtnFinsh.IsEnabled = false;
                BtnRePrint.IsEnabled = false;
                TBPackageId.IsEnabled = true;
                LabPackNum.Content = "0";
                TBTrayId.Text = "";
                TBPackageId.Text = "";
              //  LoadWarehouseConfig();
                TBWarehouse.Text = config.Get("WAREHOUSE");
                TBPosition.Text = msg.GetAllLevelMsgs().Trim();
            }
            else {
                new InfoBoard(MsgLevel.Mistake, msg.GetAllLevelMsgs()).ShowDialog();
            }
        }

        private void TBTrayId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BtnNew_Click(sender, e);
            TBTrayId.Text = "";
            TBTrayId.IsReadOnly = !TBTrayId.IsReadOnly;
            TBPackageId.IsEnabled = TBTrayId.IsReadOnly;
            BtnRePrint.IsEnabled = !TBTrayId.IsReadOnly;
            BtnNew.IsEnabled = TBTrayId.IsReadOnly;
            TBTrayId.Background = new BrushConverter().ConvertFrom(TBTrayId.IsReadOnly ? "#FFFCD2D2" : "White") as Brush; 
        }

        private void BtnRePrint_Click(object sender, RoutedEventArgs e)
        {
            this.PrintTrayLabel(TBTrayId.Text);
        }

        private void BtnTraySearch_Click(object sender, RoutedEventArgs e)
        {
            (new TraySearch()).ShowDialog();
        }
       
        private void LoadWarehouseConfig() {
            //ConfigUtil config = new ConfigUtil("STORE", "config.ini");
            //TBWarehouse.Text = config.Get("WAREHOUSE");
           // TBPosition.Text = config.Get("POSITION");
        }
        
        private void PrintTrayLabel(string trayId) {
            Message pmsg = Printer.PrintTrayLabel(trayId);
            if (pmsg.Result)
                new InfoBoard(MsgLevel.Successful, pmsg.Content,5000).ShowDialog();
            else
                new InfoBoard(MsgLevel.Mistake, pmsg.Content).ShowDialog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            for (int i = 0; i < 4000; i++)
            {
                List<string> ids = new List<string>();
                for (int j = 0; j < 10; j++) {
                    ids.Add("P" + (i *10+ j).ToString());
                }
                packageStoreService.CompleteStore(ids, "FW87", "Posi" + (i * 10).ToString());
            }
        }
    }
}
