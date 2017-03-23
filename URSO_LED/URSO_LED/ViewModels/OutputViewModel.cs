using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using URSO_LED.Connection;
using System.Net.Sockets;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using URSO_LED.Models;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
//Ctrl+K+D
namespace URSO_LED.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OutputViewModel : ViewModelBase
    {
        
        public TcpClient Client;
        public string message = "";
        public LEDSegment Segment;
        private string _segments = "Visible";
        private string _segmentName = "";
        private string _segmentType= "";
        private string _selectedConfig = "";
        private string _configName = "";
        private LEDSegment _selectedSegment;
        private ObservableCollection<String> _configList;
        private ObservableCollection<String> _typeList;
        private ObservableCollection<LEDSegment> _segmentList;
        int id = 0;
        public OutputViewModel()
        {
            RegisterMessage();
            Client = new TcpClient();
            _segmentList = new ObservableCollection<LEDSegment>();
            _configList = new ObservableCollection<String>();
            _typeList = new ObservableCollection<String>(new string[] { "PWM", "ON/OFF" });

            CommandsToMethods();
            LoadConfigurations();
        }


        #region props

        public String Segments
        {
            get
            {
                return _segments;

            }

            set
            {
                if (_segments == value)
                    return;
                _segments = value;
                RaisePropertyChanged("Segments");
            }

        }

        public String ConfigName
        {
            get
            {
                return _configName;

            }

            set
            {
                if (_configName == value)
                    return;
                _configName = value;
                RaisePropertyChanged("ConfigName");
            }

        }

        public String SegmentName
        {
            get
            {
                return _segmentName;

            }

            set
            {
                if (_segmentName == value)
                    return;
                _segmentName = value;
                RaisePropertyChanged("SegmentName");
            }

        }

        public String SegmentType
        {
            get
            {
                return _segmentType;

            }

            set
            {
                if (_segmentType == value)
                    return;
                _segmentType = value;
                RaisePropertyChanged("SegmentType");
            }

        }

        public String SelectedConfig
        {
            get
            {
                return _selectedConfig;
            }

            set
            {
                if (_selectedConfig == value)
                    return;
                _selectedConfig = value;
                LoadXml();
                RaisePropertyChanged("SelectedConfig");
            }

        }

        public LEDSegment SelectedSegment
        {
            get
            {
                return _selectedSegment;
            }

            set
            {
                if (_selectedSegment == value)
                    return;
                _selectedSegment = value;
                //LoadXml();
                RaisePropertyChanged("SelectedSegment");
            }

        }

        public ObservableCollection<String> TypeList
        {
            get
            {
                return _typeList;
            }

            set
            {
                if (_typeList == value)
                    return;
                _typeList = value;
                RaisePropertyChanged("TypeList");
            }

        }

        public ObservableCollection<String> ConfigList
        {
            get
            {
                return _configList;
            }

            set
            {
                if (_configList == value)
                    return;
                _configList = value;
                RaisePropertyChanged("ConfigList");
            }

        }

        public ObservableCollection<LEDSegment> SegmentList
        {
            get
            {
                return _segmentList;
            }

            set
            {
                if (_segmentList == value)
                    return;
                _segmentList = value;
                RaisePropertyChanged("SegmentList");
            }

        }



        #endregion

        #region commands
        public ICommand AddConfiguration { get; private set; }
        public ICommand AddSegment { get; private set; }
        public ICommand SaveConfiguration { get; private set; }
        public ICommand RemoveConfiguration { get; private set; }
        public ICommand RemoveSegment { get; private set; }
        #endregion

        #region methods

        private void LoadConfigurations()
        {
            var systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            // string[] filePaths = Directory.GetFiles(systemPath);
            ConfigList.Clear();
            foreach (var file in Directory.EnumerateFiles(systemPath, "*.xml"))
            {
                string fileName_ = file.ToString().Replace(systemPath.ToString() + "\\", "");
                fileName_ = fileName_.Replace(".xml", "");
                ConfigList.Add(fileName_);
            }
        }

        private void AddConfigurationExecute()
        {
            Segments = "Visible";
            try
            {
                /*
            NetworkStream Stream = Client.GetStream();
            if (Stream.CanWrite)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("BLK0").Append(id);
                message = builder.ToString();
                byte[] Buffer = Encoding.ASCII.GetBytes(message);
                Stream.Write(Buffer, 0, Buffer.Length);
            }
            */
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Połączenie zerwane");
            }

        }

        private void RemoveConfigurationExecute()
        {
            var systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            System.IO.DirectoryInfo di = new DirectoryInfo(systemPath);
            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Name == (SelectedConfig + ".xml"))
                {
                    file.Delete();
                    ConfigList.Remove(SelectedConfig);
                    
                }
            }
            SelectedConfig = ConfigList.FirstOrDefault();

        }

    
        private void SaveConfigurationExecute()
        {
            var systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            ListToXmlFile(systemPath + "\\" + ConfigName + ".xml");
            SegmentName = "";
            ConfigName = "";
          //  Segments = "Hidden";
            LoadConfigurations();
        }

        private void RegisterMessage()
        {
            Messenger.Default.Register<MessageOne>(this,
             (action) => ReceiveMessage(action));
        }

        private void AddSegmentExecute()
        {
            Segment = new LEDSegment();
            Segment.Id = id;
            Segment.Name = SegmentName;
            Segment.Type = SegmentType;
            SegmentList.Add(Segment);
            id++;
        }

        private void RemoveSegmentExecute()
        {
            SegmentList.Remove(SelectedSegment);
            id--;
        }


       
        private object SendMessage()
        {
            var msg = new MessageOne() { Status = true };
            Messenger.Default.Send<MessageOne>(msg);
            return null;
        }

        private object ReceiveMessage(MessageOne action)
        {
            if (action.Status == true)
                Client = action.tcpClient;

            return null;
        }


        private void LoadXml()
        {
            var systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            try
            {
                XmlFileToList(systemPath + "\\" + _selectedConfig + ".xml");
            }
            catch (FileNotFoundException)
            {

            }

        }

        private void XmlFileToList(string filepath)
        {
            SegmentList.Clear();
            using (var sr = new StreamReader(filepath))
            {
                var deserializer = new XmlSerializer(typeof(ObservableCollection<LEDSegment>));
                ObservableCollection<LEDSegment> tmpList = (ObservableCollection<LEDSegment>)deserializer.Deserialize(sr);
                foreach (var item in tmpList)
                {
                    SegmentList.Add(item);
                }
            }
            id = SegmentList.Count;
        }


        private void ListToXmlFile(string filePath)
        {
            using (var sw = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<LEDSegment>));
                serializer.Serialize(sw, SegmentList);
            }
        }

        private void CommandsToMethods()
        {
            AddConfiguration = new RelayCommand(() => AddConfigurationExecute(), () => true);
            AddSegment = new RelayCommand(() => AddSegmentExecute(), () => true);
            SaveConfiguration = new RelayCommand(() => SaveConfigurationExecute(), () => true);
            RemoveConfiguration = new RelayCommand(() => RemoveConfigurationExecute(), () => true);
            RemoveSegment = new RelayCommand(() => RemoveSegmentExecute(), () => true);
        }

        #endregion
    }
}