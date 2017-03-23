using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight.Messaging;
using URSO_LED.Connection;
using System.Net.Sockets;

namespace URSO_LED.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        private ViewModelBase _currentViewModel;
        private string _isConnected = "false";
        readonly static LEDViewModel _ledViewModel = new LEDViewModel();
        readonly static InitViewModel _initViewModel = new InitViewModel();
        readonly static ConfigViewModel _configViewModel = new ConfigViewModel();
        readonly static OutputViewModel _outputViewModel = new OutputViewModel();
        TcpClient Client;

        public MainViewModel()
        {


            CurrentViewModel = MainViewModel._initViewModel;
            LEDViewCommand = new RelayCommand(() => ExecuteLEDViewCommand());
            ConfigViewCommand = new RelayCommand(() => ExecuteConfigViewCommand());
            OutputViewCommand = new RelayCommand(() => ExecuteOutputViewCommand());
            RegisterMessage();
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }





        #region commands

       

        private void ExecuteLEDViewCommand()
        {
            CurrentViewModel=MainViewModel._ledViewModel;
        }
        private void ExecuteConfigViewCommand()
        {
            CurrentViewModel = MainViewModel._configViewModel;
        }
        private void ExecuteOutputViewCommand()
        {
            CurrentViewModel = MainViewModel._outputViewModel;
        }
        #endregion

        #region props
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

       
        public String IsConnected
        {
            get
            {
                return _isConnected;

            }

            set
            {
                if (_isConnected == value)
                    return;
                _isConnected = value;
                RaisePropertyChanged("IsConnected");
            }

        } 

        #endregion


        #region commands
        public ICommand LEDViewCommand { get; private set; }
        public ICommand ConfigViewCommand { get; private set; }
        public ICommand OutputViewCommand { get; private set; }
        #endregion

        #region methods
        private void RegisterMessage()
        {
            Messenger.Default.Register<MessageOne>(this,
             (action) => ReceiveMessage(action));
        }

        private object ReceiveMessage(MessageOne action)
        {
            if (action.Status == true)
                Client = action.tcpClient;

            if (Client.Connected == true)
                IsConnected = "true";
            return null;
        }

        #endregion



    }
}