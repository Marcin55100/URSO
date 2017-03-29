using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URSO_LED.Models
{
    public class LEDSegment : INotifyPropertyChanged
    {
        private string _pwm="";
        public event PropertyChangedEventHandler PropertyChanged;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Blink { get; set; } = "White";

        public string PWM
        {
            get { return _pwm; }

            set
            {
                if (value == _pwm)
                    return;
                _pwm = value;
                NotifyPropertyChanged("PWM");

            }
        }
        public string ONOff { get; set; }


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }

        }

    }
}
