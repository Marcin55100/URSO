﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace URSO_LED.Connection
{
    public class MessageOne
    {
       // public TcpClient tcpClient { get; set; }
        public TcpClient tcpClient { get; set; }
        public bool Status { get; set; }
    }
}
