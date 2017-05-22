using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight.Messaging;
using URSO_LED.Connection;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.ComponentModel;

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
        private ObservableCollection<MenuItems> _menuList;
        // readonly static LEDViewModel _ledViewModel = new LEDViewModel();
        // readonly static InitViewModel _initViewModel = new InitViewModel();
        // readonly static ConfigViewModel _configViewModel = new ConfigViewModel();
        // readonly static OutputViewModel _outputViewModel = new OutputViewModel();
        TcpClient Client;

        public class MenuItems:INotifyPropertyChanged
        {
            private string _text="";
            private string _background="";


            public event PropertyChangedEventHandler PropertyChanged;
            public string Background
            {
                get { return _background; }

                set
                {
                    if (value == _background)
                        return;
                    _background = value;
                    NotifyPropertyChanged("Background");

                }
            }
            public string Text
            {
                get { return _text; }

                set
                {
                    if (value == _text)
                        return;
                    _text = value;
                    NotifyPropertyChanged("Text");

                }
            }




            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

                }

            }

        }


        MenuItems _menuItem;
        public MainViewModel()
        {
            // CurrentViewModel = _initViewModel;
            //LEDViewCommand = new RelayCommand(() => ExecuteLEDViewCommand());
            //ConfigViewCommand = new RelayCommand(() => ExecuteConfigViewCommand());
            //OutputViewCommand = new RelayCommand(() => ExecuteOutputViewCommand());
            SelectionChangedCommand = new RelayCommand<object>((s) => ExecuteSelectionChangedCommand(s));
            RegisterMessage();
            _menuList = new ObservableCollection<MenuItems>();

            _menuItem = new MenuItems() { Text="LED", Background="Red" };
            MenuList.Add(_menuItem);
            _menuItem = new MenuItems() { Text = "Konfiguracja", Background = "Blue" };
            MenuList.Add(_menuItem);
            _menuItem = new MenuItems() { Text = "Ustawienia portów", Background = "Blue" };
            MenuList.Add(_menuItem);
        }




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

        public ObservableCollection<MenuItems> MenuList
        {
            get
            {
                return _menuList;
            }
            set
            {
                if (value == _menuList)
                    return;

                _menuList = value;
                RaisePropertyChanged("MenuList");
            }
        }

        public string IsConnected
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
        //public ICommand LEDViewCommand { get; private set; }
        //public ICommand ConfigViewCommand { get; private set; }
        //public ICommand OutputViewCommand { get; private set; }
        public RelayCommand<object> SelectionChangedCommand { get; private set; }
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
            {
                IsConnected = "true";
                changeLEDColor(true);
            }
            else
            {
                IsConnected = "false";
                changeLEDColor(false);
            }

            return null;
        }
        private void changeLEDColor(bool connected)
        {
            if(connected==true)
                MenuList.First().Background = "Green";
            else
                MenuList.First().Background = "Red";
        }



        private void ExecuteSelectionChangedCommand(object parameter)
        {
            var listBox = parameter as ListBox;
            var item = listBox.SelectedItem as MenuItems;

            if (item.Text == "LED")
                ExecuteLEDViewCommand();
            else if (item.Text == "Konfiguracja")
                ExecuteConfigViewCommand();
            else if (item.Text == "Ustawienia portów")
                ExecuteOutputViewCommand();
        }

        private void ExecuteLEDViewCommand()
        {
            // CurrentViewModel=MainViewModel._ledViewModel;
            var msg = new GoToPageMessage() { PageName = "LEDView" };
            Messenger.Default.Send<GoToPageMessage>(msg);

        }
        private void ExecuteConfigViewCommand()
        {
            // CurrentViewModel = MainViewModel._configViewModel;
            var msg = new GoToPageMessage() { PageName = "ConfigView" };
            Messenger.Default.Send<GoToPageMessage>(msg);
        }
        private void ExecuteOutputViewCommand()
        {
            // CurrentViewModel = MainViewModel._outputViewModel;
            var msg = new GoToPageMessage() { PageName = "OutputView" };
            Messenger.Default.Send<GoToPageMessage>(msg);
        }



        #endregion
    }
}