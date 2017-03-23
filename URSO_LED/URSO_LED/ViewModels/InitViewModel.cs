using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Net.Sockets;
using URSO_LED.Connection;
using System;
using System.Threading.Tasks;
using System.Timers;
using SimpleWifi;

namespace URSO_LED.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InitViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the InitViewModel class.
        /// </summary>
        /// 
        public string _connectionInfo = "Łączę";
        public TcpClient Client;
        static Timer _timer;
        public bool flag = false;
        public InitViewModel()
        {
            Messenger.Default.Register<MessageOne>(this,
             (action) => ReceiveMessage(action));

            //Client = new TcpClient();
            //if ((ConnectionControl.ConnectBluegiga(Client,new Wifi()) == true))
            //{
            //    ConnectionInfo = "Połączono";
            //    Start();
            //}
        }


        #region props
        public string ConnectionInfo
        {

            get
            {
                return _connectionInfo;
            }


            set
            {
                if (_connectionInfo == value)
                    return;
                _connectionInfo = value;
                RaisePropertyChanged("ExampleValue");
            }
        }
        #endregion

        #region methods
        private void Start()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;
            // Enable it    
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (flag == false)
            {
                //SendClient(Client);
                flag = true;
                _timer.Stop();
            }
        }

        private object ReceiveMessage(MessageOne action)
        {
            return null;
        }

        #endregion

    }
}