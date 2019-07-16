using FileMover.Utility;
using FileMover.ViewModel;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;


namespace FileMover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        public MainWindow()
        {
            XmlWritter xml = new XmlWritter();
            xml.createXml();

            ViewModelMain vmm = new ViewModelMain();
            DataContext = vmm;
            InitializeComponent();
                   
            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            MyNotifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/img/move.ico")).Stream);
            MyNotifyIcon.Visible = true;

            MyNotifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDown);     
        }

        void MyNotifyIcon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");
                menu.IsOpen = true;
            }
            else
            {
                if (WindowState == System.Windows.WindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                }
            }
        }
        private void Menu_Open(Object sender, RoutedEventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }
        private void Menu_Close(Object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
                MyNotifyIcon.BalloonTipTitle = "Application Minimized";
                MyNotifyIcon.BalloonTipText = "FileMover App";
                MyNotifyIcon.Text = "FileMover";
                MyNotifyIcon.ShowBalloonTip(100);
            }
            base.OnStateChanged(e);
        }     

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    this.WindowState = WindowState.Minimized;
        //    base.OnClosing(e);
        //    e.Cancel = true;
        //}
    }
}
