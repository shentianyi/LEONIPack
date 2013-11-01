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
using System.IO;
using System.Timers;
namespace Brilliantech.Packaging.Store.UI
{
    /// <summary>
    /// InfoBoard.xaml 的交互逻辑
    /// </summary>
    public partial class InfoBoard : Window
    {
        public InfoBoard()
        {
            InitializeComponent();
        }
        private static readonly string infoImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageMedia\\Info.png");
        private static readonly string warningImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageMedia\\Warning.png");
        private static readonly string successImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageMedia\\ok.png");
        private static readonly string errorImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageMedia\\Error.png");
        
        private System.Timers.Timer timer;
      
        public InfoBoard(MsgLevel level, string msg)
        {
            InitializeComponent();
            this.AssignPicture(level);
            this.AssignText(msg);
        }

        public InfoBoard(MsgLevel level, string msg, int autoCloseInterval)
        {
            InitializeComponent();
            this.AssignPicture(level);
            this.AssignText(msg);

            timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_elsped);
            timer.Interval = autoCloseInterval;
            timer.Start();
        }
        public InfoBoard(MsgLevel level, string[] msg)
        {
            InitializeComponent();
            this.AssignPicture(level);
            this.AssignText(ArrayToString(msg));
        }

        public InfoBoard(MsgLevel level, string[] msg, int autoCloseInterval)
        {
            InitializeComponent();
            this.AssignPicture(level);
            this.AssignText(ArrayToString(msg));

            timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_elsped);
            timer.Interval = autoCloseInterval;
            timer.Start();
        }
        private void timer_elsped(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Dispatcher.Invoke(new CloseMyself(CloseSub));
        }

        private delegate void CloseMyself();

        private void CloseSub()
        {
            this.DialogResult = true;
            this.Close();
        }

        public void AssignPicture(MsgLevel level)
        {
            string imagePath = "";
            try
            {
                switch (level)
                {
                    case MsgLevel.Info:
                        imagePath = infoImage;
                        break; // TODO: might not be correct. Was : Exit Select
                    case MsgLevel.Mistake:
                        imagePath = errorImage;
                        break; // TODO: might not be correct. Was : Exit Select

                    case MsgLevel.Successful:
                        imagePath = successImage;
                        break; // TODO: might not be correct. Was : Exit Select

                    case MsgLevel.Warning:
                        imagePath = warningImage;
                        break; // TODO: might not be correct. Was : Exit Select

                }
                this.Indicator.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

            }
            catch (Exception ex)
            {
            }


        }


        private void AssignText(string msg)
        {
            this.TextBox_Message.Text = msg;
        }

        static char vbCr=Convert.ToChar(13);
        static char vbLf =Convert.ToChar(12);
        public static string ArrayToString(string[] str)
        {
            string combined = "";
            if (str != null)
            {
                if (str.Length > 0)
                {
                    foreach (string st in str)
                    {
                        combined = combined +vbCr+vbLf + st;
                    }
                }

            }
            return combined.TrimStart(new char[] { vbCr,vbLf });
        }

        private void Button_yes_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.timer.Stop();
            }
            catch (Exception ex)
            {
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Button_no_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.timer.Stop();
            }
            catch (Exception ex)
            {
            }

            this.DialogResult = false;
            this.Close();
        }


        private void Window_Loaded(System.Object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
