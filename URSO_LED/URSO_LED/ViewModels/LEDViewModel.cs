using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using URSO_LED.Connection;
using URSO_LED.Models;

namespace URSO_LED.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LEDViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the LEDViewModel class.
        /// </summary>
        /// 

        private ObservableCollection<String> _configList;
        private ObservableCollection<LEDSegment> _segmentList;
        private string _selectedConfig;
        public TcpClient Client;
        public LEDViewModel()
        {
            RegisterMessage();
            Client = new TcpClient();
            _configList = new ObservableCollection<String>();
            _segmentList = new ObservableCollection<LEDSegment>();
            LoadConfigurations();
            OnLoaded = new RelayCommand(() => OnLoadedExecute(), () => true);
            LEDBtnClicked = new RelayCommand<object>((s) => LEDBtnClickedExecute(s));
            SliderChanged = new RelayCommand(() => SliderChangedExecute());

            LoadXml();
        }


        #region props
        public String SelectedConfig
        {
            get
            {
                return _selectedConfig;
            }
            set
            {
                if (value == _selectedConfig)
                    return;
                _selectedConfig = value;
                LoadXml();
                RaisePropertyChanged("SelectedConfig");

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
        public ICommand OnLoaded { get; private set; }
        public RelayCommand<object> LEDBtnClicked { get; private set; }
        public ICommand SliderChanged { get; private set; }





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
                    if (item.Type == "PWM")
                    { 
                        item.ONOff = "0";
                        item.PWM = "100";
                    }
                    else
                    { 
                    item.ONOff = "Auto";
                    item.PWM = "0";
                    }

                    SegmentList.Add(item);
                    }
                }
            }
        private void OnLoadedExecute()
        {
            LoadConfigurations();
        }

        //private void LEDOnExecute(int ledNumber)
        //{
        //    SendPacket("LON", ledNumber.ToString());
        //}

        private void SliderChangedExecute()
        {
            string name = "marcin";
        }


        private void LEDBtnClickedExecute(object clickedButton)
        {
            Button button = clickedButton as Button;

            if(button.Name=="ledONButton")
                SendPacket("LON", button.Tag.ToString());
            else if(button.Name == "ledOFFButton")
                SendPacket("LOF", button.Tag.ToString());
        }

        private void SendPacket(String Command, String Number)
        {
            string message;
            if (Client.Connected)
            {
                NetworkStream Stream = Client.GetStream();
                if (Stream.CanWrite)
                {
                    try
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(Command).Append(0).Append(Number);
                         message = builder.ToString();
                          byte[] Buffer = Encoding.ASCII.GetBytes(message);
                         Stream.Write(Buffer, 0, Buffer.Length);
                    }
                    catch (SystemException)
                    {
                        MessageBox.Show("Połączenie przerwane.");
                    }
                }
            }
        }









        private void RegisterMessage()
        {
            Messenger.Default.Register<MessageOne>(this,
             (action) => ReceiveMessage(action));
        }

        private object ReceiveMessage(MessageOne action)
        {
            if (action.Status == true)
                Client = action.tcpClient;

            return null;
        }


        #endregion








    }
}

