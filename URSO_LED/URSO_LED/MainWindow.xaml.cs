using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using URSO_LED.Connection;
using URSO_LED.ViewModels;
using URSO_LED.Views;

namespace URSO_LED
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            Messenger.Default.Register<GoToPageMessage>
                (
                    this,
                    (action) => ReceiveMessage(action)
                );
            this.contentControl1.Content = this.InitView;

            _configView = new ConfigView();
            _outputView = new OutputView();
            _ledView = new LEDView();
            _initView = new InitView();
        }
        #region props
        private OutputView _outputView;
        private OutputView OutputView
        {
            get
            {
                if (_outputView == null)
                    _outputView = new OutputView();
                return _outputView;
            }
        }

        private LEDView _ledView;
        private LEDView LEDView
        {
            get
            {
                if (_ledView == null)
                    _ledView = new LEDView();
                return _ledView;
            }
        }

        private InitView _initView;
        private InitView InitView
        {
            get
            {
                if (_initView == null)
                    _initView = new InitView();
                return _initView;
            }
        }

        private ConfigView _configView;
        private ConfigView ConfigView
        {
            get
            {
                if (_configView == null)
                    _configView = new ConfigView();
                return _configView;
            }
        }

        #endregion

        #region methods
        private object ReceiveMessage(GoToPageMessage action)
        {
            //            this.contentControl1.Content = this.Page2View;
            //this shows what pagename property is!!
            switch (action.PageName)
            {
                case "ConfigView":
                    if (this.contentControl1.Content != this.ConfigView)
                        this.contentControl1.Content = this.ConfigView;
                    break;
                case "InitView":
                    if (this.contentControl1.Content != this.InitView)
                        this.contentControl1.Content = this.InitView;
                    break;
                case "LEDView":
                    if (this.contentControl1.Content != this.LEDView)
                        this.contentControl1.Content = this.LEDView;
                    break;
                case "OutputView":
                    if (this.contentControl1.Content != this.OutputView)
                        this.contentControl1.Content = this.OutputView;
                    break;
                default:
                    break;
            }

            //            string testII = action.PageName.ToString();
            //           System.Windows.MessageBox.Show("You were successful switching to " + testII + ".");

            return null;
        }



        #endregion


        private void Window_ContentRendered(object sender, EventArgs e)
        {
            ConnectionControl.CreateConnectionComponents();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            
        }
    }
}
