using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SimpleWifi;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using URSO_LED.Models;
using URSO_LED.Connection;
using System.Net.Sockets;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
//Do zrobienia:
//PasswordBox
//Service Mode
//
namespace URSO_LED.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ConfigViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ConfigViewModel class.
        /// </summary>

        private ObservableCollection<WifiNetwork> _networkList;
        public WifiNetwork _selectedNetwork;
        Wifi wifi;
        TcpClient Client;

        public ConfigViewModel()
        {
            RegisterMessage();
            _networkList = new ObservableCollection<WifiNetwork>();
            CommandsToMethods();
        }
        #region props

        public ObservableCollection<WifiNetwork> NetworkList
        {
            get
            {
                return _networkList;

            }
            set
            {
                if (value == _networkList)
                    return;

                _networkList = value;
                RaisePropertyChanged("NetworkList");
            }

        }

        public String Password { get; set; }
        public String ConnectionStatus { get; set; } = "Łączę ze sterownikiem...";
        public WifiNetwork SelectedNetwork
        {
            get { return _selectedNetwork; }


            set
            {
                if (value == _selectedNetwork)
                    return;
                _selectedNetwork = value;
                RaisePropertyChanged("SelectedNetwork");
            }

        }
        public String Bold { get; set; } = "Bold";

        #endregion

        #region commands

        public ICommand ConnectToNetwork { get; private set; }
        public ICommand RefreshWifi { get; private set; }
        #endregion

        #region methods

        private void WifiSearch(Wifi wifi)
        {
            NetworkList.Clear();
            string fontValue = "Normal";
            if (!wifi.NoWifiAvailable)
            {
                foreach (var accessPoint in wifi.GetAccessPoints())
                {
                    fontValue = "Normal";
                    if (accessPoint.IsConnected) fontValue = "Bold";
                    NetworkList.Add(new WifiNetwork { Name = accessPoint.Name, Font= fontValue });
                }
            }
        }

        private void ConnectToNetworkExecute()
        {
            string password = "";
            if (wifi.GetAccessPoints().Find(item => item.Name == SelectedNetwork.Name).IsSecure)
            {
                password = Password;
            }
            ConnectionControl.ConnectNetwork(wifi, SelectedNetwork.Name, password);

            if (wifi.ConnectionStatus == WifiStatus.Connected)
            {
                if (wifi.GetAccessPoints().Find(item => item.IsConnected).Name == SelectedNetwork.Name)
                {
                    ConnectionControl.DeleteMemory();
                    if (ConnectionControl.ConnectBluegiga(Client, wifi))
                    {
                        MessageBox.Show("Połączono.");
                        ConnectionStatus = "Połączono";
                    }
                    else
                    {
                        MessageBox.Show("Brak połączenia.");
                        ConnectionStatus = "Brak połączenia.";
                    }
                }
            }
            WifiSearch(wifi);
        }


        private void wifi_ConnectionStatusChanged(object sender, WifiStatusEventArgs e)
        {
            //WifiSearch(wifi);
            //automatyczne odświeżanie listy sieci
        }


        private void RegisterMessage()
        {
            Messenger.Default.Register<MessageOne>(this,
             (action) => ReceiveMessage(action));
        }

        private object ReceiveMessage(MessageOne action)
        {
            if (action.Status == true)
            {
                Client = action.tcpClient;
                wifi = action.wifi;
                if (Client.Connected)
                    ConnectionStatus = "Połączono.";
                else
                    ConnectionStatus = "Brak połączenia.";
                WifiSearch(wifi);
            }
            return null;
        }

        private void RefreshWifiExecute()
        {
            WifiSearch(wifi);
        }

        private void CommandsToMethods()
        {
            ConnectToNetwork = new RelayCommand(() => ConnectToNetworkExecute(), () => true);
            RefreshWifi = new RelayCommand(() => RefreshWifiExecute(), () => true);
        }

        #endregion

    }
}
