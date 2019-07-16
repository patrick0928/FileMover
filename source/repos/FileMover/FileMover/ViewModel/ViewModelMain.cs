using FileMover.Helper;
using FileMover.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;

namespace FileMover.ViewModel
{
    public class ViewModelMain : ViewModelBase
    {
        bool _isEnable;
        bool _isSuspended;
        Visibility _isVisible;
        Visibility _showLogs;
        ImageSource _arrowImage;
        string _sourcePath1;
        string _sourcePath2;
        string _sourcePath3;
        int _windowWidth;
        int _windowMinWidth;
        int _columnWidth;
        private string _fileType1;
        private string _fileType2;
        private string _fileType3;
        string _destinationPath;
        private string _duration;
        private readonly CollectionView _time;
        private readonly CollectionView _logs;
        DispatcherTimer CountdownTimer;
        public RelayCommand GetPathCommand { get; set; }
        public RelayCommand StartButtonCommand { get; set; }
        public RelayCommand SuspendButtonCommand { get; set; }
        public RelayCommand ShowLogsCommand { get; set; }
        public RelayCommand ClearLogsCommand { get; set; }
        private ObservableCollection<Log> _logCollection = new ObservableCollection<Log>();
        private ObservableCollection<FileType> _filesCollection = new ObservableCollection<FileType>();
        private ObservableCollection<TimeComboBox> _timeCollection = new ObservableCollection<TimeComboBox>();

        #region Generate FileType to ComboBox

        //public static System.Windows.Resources.StreamResourceInfo streamInfo()
        //{
        //    Uri uri = new Uri(@"C:\FileMover\config.xml", UriKind.Relative);
        //    System.Windows.Resources.StreamResourceInfo info = Application.GetResourceStream(uri);

        //    return info;
        //}

        public static string[] getData()
        {
            //var document = XDocument.Load(@"C:\Users\ps-it4\source\repos\FileMover\FileMover\bin\config.xml");          
            string[] fileType = XDocument.Load(@"C:\FileMover\config.xml").Descendants("type")
                                        .Select(element => element.Value).ToArray();
            return fileType;
        }

        public CollectionView Logs
        {
            get { return _logs; }
        }
        public string FileType1
        {
            get { return _fileType1; }
            set
            {
                if (_fileType1 != value)
                _fileType1 = value;
                RaisePropertyChanged("FileType1");
            }
        }
        public string FileType2
        {
            get { return _fileType2; }
            set
            {
                if (_fileType2 != value)
                _fileType2 = value;
                RaisePropertyChanged("FileType2");
            }
        }
        public string FileType3
        {
            get { return _fileType3; }
            set
            {
                if (_fileType3 != value)
                _fileType3 = value;
                RaisePropertyChanged("FileType3");
            }
        }
        #endregion
        #region Generate TimeDuration to ComboBox
        public static string[] getTimeData()
        {
            string[] fileType = XDocument.Load(@"C:\FileMover\config.xml").Descendants("duration")
                                       .Select(element => element.Value).ToArray();
            return fileType;
        }
        public CollectionView Time
        {
            get { return _time; }
        }

        public string TimeComboBox
        {
            get { return _duration; }
            set
            {
                //if (_duration != value) return;
                _duration = value;
                RaisePropertyChanged("TimeComboBox");
                //timeSelectionChange(TimeComboBox);
            }
        }
        #endregion
        #region Properties
        public ObservableCollection<Log> LogCollection
        {
            get { return _logCollection; }
            set { _logCollection = value; }
        }

        public ObservableCollection<FileType> FileCollection
        {
            get { return _filesCollection; }
            set { _filesCollection = value; }
        }

        public ObservableCollection<TimeComboBox> TimeCollection
        {
            get { return _timeCollection; }
            set { _timeCollection = value; }
        }

        public string SourcePath1
        {
            get { return _sourcePath1; }
            set
            {
                if (_sourcePath1 != value)
                {
                    _sourcePath1 = value;
                    RaisePropertyChanged("SourcePath1");
                }
            }
        }
        public string SourcePath2
        {
            get { return _sourcePath2; }
            set
            {
                if (_sourcePath2 != value)
                {
                    _sourcePath2 = value;
                    RaisePropertyChanged("SourcePath2");
                }
            }
        }
        public string SourcePath3
        {
            get { return _sourcePath3; }
            set
            {
                if (_sourcePath3 != value)
                {
                    _sourcePath3 = value;
                    RaisePropertyChanged("SourcePath3");
                }
            }
        }
        public string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                if (_destinationPath != value)
                {
                    _destinationPath = value;
                    RaisePropertyChanged("DestinationPath");
                }
            }
        }
        public string Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    RaisePropertyChanged("Time");
                }
            }
        }
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        public bool IsSuspended
        {
            get { return _isSuspended; }
            set
            {
                _isSuspended = value;
                RaisePropertyChanged("IsSuspended");
            }
        }

        public Visibility IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }
        public Visibility ShowLogs
        {
            get { return _showLogs; }
            set
            {
                _showLogs = value;
                RaisePropertyChanged("ShowLogs");
            }
        }

        public int WindowMinWidth
        {
            get { return _windowMinWidth; }
            set
            {
                _windowMinWidth = value;
                RaisePropertyChanged("WindowMinWidth");
            }
        }

        public int WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                RaisePropertyChanged("WindowWidth");
            }
        }
        public int ColumnWidth
        {
            get { return _columnWidth; }
            set
            {
                _columnWidth = value;
                RaisePropertyChanged("ColumnWidth");
            }
        }

        public ImageSource ArrowImage
        {
            get { return _arrowImage; }
            set
            {
                _arrowImage = value;
                RaisePropertyChanged("ArrowImage");
            }
        }
        #endregion
        #region Methods
        void getSource(object parameter)
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = folderBrowser.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (parameter.ToString() == "SourcePath1")
                    SourcePath1 = folderBrowser.SelectedPath.ToString();

                else if (parameter.ToString() == "SourcePath2")
                    SourcePath2 = folderBrowser.SelectedPath.ToString();

                else if (parameter.ToString() == "SourcePath3")
                    SourcePath3 = folderBrowser.SelectedPath.ToString();

                else if (parameter.ToString() == "DestinationPath")
                    DestinationPath = folderBrowser.SelectedPath.ToString();
            }
        }
        public int timeSpan(int time)
        {
            if (time == 1)
                return 60;
            else if (time == 2)
                return 120;
            else if (time == 3)
                return 180;
            else if (time == 4)
                return 240;
            else if (time == 5)
                return 300;
            else
                return 360;
        }
        void timer_tick(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(SourcePath1) && !string.IsNullOrEmpty(DestinationPath))
            {
                try { moveFile(Directory.GetFiles(SourcePath1), "Path 1", FileType1); }
                catch (IOException ex)
                {
                    MessageBox.Show("Invalid Source Path 1");
                    CountdownTimer.Stop();
                    IsEnable = true;
                    IsSuspended = false;
                    IsVisible = Visibility.Hidden;
                }
            }

            if (!string.IsNullOrEmpty(SourcePath2) && !string.IsNullOrEmpty(DestinationPath))
            {
                try { moveFile(Directory.GetFiles(SourcePath2), "Path 2", FileType2); }
                catch (IOException ex)
                {
                    MessageBox.Show("Invalid Source Path 2");
                    CountdownTimer.Stop();
                    IsEnable = true;
                    IsSuspended = false;
                    IsVisible = Visibility.Hidden;
                }
            }

            if (!string.IsNullOrEmpty(SourcePath3) && !string.IsNullOrEmpty(DestinationPath))
            {
                try { moveFile(Directory.GetFiles(SourcePath3), "Path 3", FileType3); }
                catch (IOException ex)
                {
                    MessageBox.Show("Invalid Source Path 3");
                    CountdownTimer.Stop();
                    IsEnable = true;
                    IsSuspended = false;
                    IsVisible = Visibility.Hidden;
                }
            }
        }

        void moveFile(string[] sourceFiles, string path, string type)
        {
            foreach (string file in sourceFiles)
            {
                string filename = System.IO.Path.GetFileName(file);

                if (filename.Contains(type))
                {
                    string destFile = System.IO.Path.Combine(DestinationPath, filename);
                    try
                    {
                        File.Move(file, destFile);
                        LogCollection.Add(new Log { Logs = filename + " Transfered from " + path });
                    }
                    catch (FileNotFoundException ex) { }
                    catch (IOException ex) { }
                }
            }
        }
        void StartFileTrasfer(object parameter)
        {
            if (DestinationPath != null)
            {
                if (SourcePath1 != null || SourcePath2 != null || SourcePath3 != null)
                {
                    try
                    {
                        Directory.GetFiles(DestinationPath);
                        CountdownTimer = new DispatcherTimer();
                        CountdownTimer.Interval = TimeSpan.FromSeconds(Int32.Parse(TimeComboBox.Substring(0, 1)));
                        CountdownTimer.Tick += new EventHandler(timer_tick);
                        CountdownTimer.Start();
                        IsEnable = false;
                        IsSuspended = true;
                        IsVisible = Visibility.Visible;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Invalid Destination Path");
                    }
                }
                else
                    MessageBox.Show("Please Select Source Path");
            }
            else
                MessageBox.Show("Please Set Destination Path");
        }

        void suspendTransfer(object parameter)
        {
            CountdownTimer.Stop();
            IsEnable = true;
            IsSuspended = false;
            IsVisible = Visibility.Hidden;
        }

        void OpenLogs(object parameter)
        {
            if (ShowLogs == Visibility.Visible)
            {
                ArrowImage = new BitmapImage(new Uri("pack://application:,,,/img/left.png"));
                ShowLogs = Visibility.Hidden;
                WindowMinWidth = 500;
                WindowWidth = 500;
                ColumnWidth = 0;
            }
            else
            {
                ArrowImage = new BitmapImage(new Uri("pack://application:,,,/img/right.png"));
                ShowLogs = Visibility.Visible;
                WindowMinWidth = 650;
                WindowWidth = 650;
                ColumnWidth = 150;
            }
        }

        void ClearLogs(object parameter)
        {
            LogCollection.Clear();
        }

        #endregion
        public ViewModelMain()
        {
            WindowMinWidth = 500;
            WindowWidth = 500;
            ColumnWidth = 0;

            IsEnable = true;
            IsSuspended = false;
            IsVisible = Visibility.Hidden;
            ShowLogs = Visibility.Hidden;
            ArrowImage = new BitmapImage(new Uri("pack://application:,,,/img/left.png"));

            //OnClick Event
            GetPathCommand = new RelayCommand(getSource);
            StartButtonCommand = new RelayCommand(StartFileTrasfer);
            SuspendButtonCommand = new RelayCommand(suspendTransfer);
            ShowLogsCommand = new RelayCommand(OpenLogs);
            ClearLogsCommand = new RelayCommand(ClearLogs);

            foreach (var item in getData())
            {
                FileCollection.Add(new FileType { Type = item,
                                                  Type2 = item,
                                                  Type3 = item });
            }

            //Fill TimeComboBox with data from xml file
            foreach (var item in getTimeData())
            {
                TimeCollection.Add(new TimeComboBox { Duration = item });
            }
        }        
    }
}
