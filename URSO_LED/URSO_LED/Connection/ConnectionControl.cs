﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SimpleWifi;
using System.IO;
using GalaSoft.MvvmLight.Messaging;

namespace URSO_LED.Connection
{
    class ConnectionControl
    {
        const string defaultAP = "xBluegiga";
        const string defaultPW = "bluegiga";
        const string memoryFile = @"\ConnectionInfo.txt";
        const int port = 54069;

        public static bool ConnectBluegiga(TcpClient tcp, Wifi wifi)
        {
            bool connection = false;
            bool networksAvailable = false;

            try
            {
                networksAvailable = wifi.GetAccessPoints().Any();
            }
            catch { }

            if (!tcp.Connected)
            {
                if (!wifi.NoWifiAvailable && networksAvailable)
                {
                    IPAddress IP;
                    string ssid;
                    if (ReadMemory(out IP, out ssid))
                    {
                        if (wifi.ConnectionStatus == WifiStatus.Connected)
                        {
                            if (wifi.GetAccessPoints().Find(item => item.IsConnected).Name == ssid)
                            {
                                tcp = CreateTCPConnection(IP, port, tcp);
                                if (tcp.Connected) connection = true;
                            }
                            else if (wifi.GetAccessPoints().Exists(item => item.Name == ssid))
                            {
                                ConnectNetwork(wifi, ssid);
                                if (wifi.ConnectionStatus == WifiStatus.Connected)
                                {
                                    if (wifi.GetAccessPoints().Find(item => item.IsConnected).Name == ssid)
                                    {
                                        tcp = CreateTCPConnection(IP, port, tcp);
                                        if (tcp.Connected) connection = true;
                                    }
                                }
                            }
                            else
                            {
                                tcp = CreateTCPConnection(IPAddress.Any, port, tcp);
                                if (tcp.Connected) connection = true;
                            }
                        }
                        else if (wifi.ConnectionStatus == WifiStatus.Disconnected)
                        {
                            if (wifi.GetAccessPoints().Exists(item => item.Name == ssid))
                            {
                                ConnectNetwork(wifi, ssid);
                                if (wifi.ConnectionStatus == WifiStatus.Connected)
                                {
                                    if (wifi.GetAccessPoints().Find(item => item.IsConnected).Name == ssid)
                                    {
                                        tcp = CreateTCPConnection(IP, port, tcp);
                                        if (tcp.Connected) connection = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (wifi.ConnectionStatus == WifiStatus.Connected)
                    {
                        Console.WriteLine("---------------------1");
                        tcp = CreateTCPConnection(IPAddress.Any, port, tcp);
                        if (tcp.Connected) connection = true;
                        else
                        {
                            if (wifi.GetAccessPoints().Exists(item => item.Name == defaultAP))
                            {
                                ConnectNetwork(wifi, defaultAP, defaultPW);
                                if (wifi.ConnectionStatus == WifiStatus.Connected)
                                {
                                    if (wifi.GetAccessPoints().Find(item => item.IsConnected).Name == defaultAP)
                                    {
                                        tcp = CreateTCPConnection(IPAddress.Any, port, tcp);
                                        if (tcp.Connected) connection = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (wifi.ConnectionStatus == WifiStatus.Disconnected)
                    {
                        Console.WriteLine("----------------" + wifi.ConnectionStatus.ToString());
                        if (wifi.GetAccessPoints().Exists(item => item.Name == defaultAP))
                        {
                            ConnectNetwork(wifi, defaultAP, defaultPW);
                            if (wifi.ConnectionStatus == WifiStatus.Connected)
                            {
                                if (wifi.GetAccessPoints().Find(item => item.IsConnected).Name == defaultAP)
                                {
                                    tcp = CreateTCPConnection(IPAddress.Any, port, tcp);
                                    if (tcp.Connected) connection = true;
                                }
                            }
                        }
                    }
                }
            }
            else connection = true;
            if (connection) SaveMemory(((IPEndPoint)tcp.Client.RemoteEndPoint).Address, wifi.GetAccessPoints().Find(item => item.IsConnected).Name);

            return connection;
        }

        private static bool ReadMemory(out IPAddress IP, out string ssid)
        {
            bool memory = true;
            var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            try
            {
                StreamReader file = new StreamReader(systemPath + memoryFile);
                memory = IPAddress.TryParse(file.ReadLine(), out IP);
                ssid = file.ReadLine();
                file.Close();
            }
            catch (Exception)
            {
                IP = IPAddress.None;
                ssid = "";
                memory = false;
            }
            Console.WriteLine("------------------" + memory.ToString());
            return memory;
        }

        public static void SaveMemory(IPAddress IP, string ssid)
        {
            var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            using (StreamWriter outputFile = new StreamWriter(systemPath + memoryFile))
            {
                outputFile.WriteLine(IP.ToString());
                outputFile.WriteLine(ssid);
            }
        }

        public static void DeleteMemory()
        {
            var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (File.Exists(systemPath + memoryFile)) File.Delete(systemPath + memoryFile);
        }

        private static TcpClient CreateTCPConnection(IPAddress IP, int port, TcpClient tcp)
        {
            var delay = Task.Run(async delegate
            {
                await Task.Delay(100);
            });
            delay.Wait();

            try
            {
                var task = tcp.ConnectAsync(IP, port);
                task.Wait();
            }
            catch (Exception)
            {
                try
                {
                    IP = UDPListener();
                    var task = tcp.ConnectAsync(IP, port);
                    task.Wait();
                }
                catch (Exception) { }
            }
            Console.WriteLine("---------------------2");
            return tcp;
        }

        private static IPAddress UDPListener()
        {
            const int listenPort = 11000;
            IPAddress ServerIP = IPAddress.Any;

            UdpClient udp = new UdpClient(listenPort);
            udp.Client.ReceiveTimeout = 2000;
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            IPEndPoint broadcast = new IPEndPoint(IPAddress.Broadcast, port);
            udp.Send(new byte[] { 1, 2, 3, 4, 5 }, 5, broadcast);
            try
            {
                byte[] bytes = udp.Receive(ref groupEP);
                string response = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                if (response == "HELLO")
                    ServerIP = IPAddress.Parse(groupEP.ToString().Split(':')[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                udp.Close();
            }
            return ServerIP;
        }

        public static void ConnectNetwork(Wifi wifi, string ssid, string password = "")
        {
            if (!wifi.NoWifiAvailable)
            {
                var accessPoint = wifi.GetAccessPoints().Find(item => item.Name == ssid);
                AuthRequest authRequest = new AuthRequest(accessPoint);
                bool overwrite = true;
                if (authRequest.IsPasswordRequired)
                {
                    if (password == "" && accessPoint.HasProfile) overwrite = false;
                    else authRequest.Password = password;
                }
                accessPoint.Connect(authRequest, overwrite);
            }
        }

        public static void CreateConnectionComponents()
        {
            TcpClient tcp = new TcpClient();
            Wifi wifi = new Wifi();
            ConnectBluegiga(tcp, wifi);
            SendClient(tcp, wifi);
        }

        public static object SendClient(TcpClient Client, Wifi wifi)
        {
            var msg = new MessageOne() { tcpClient = Client, wifi = wifi, Status = true };
            Messenger.Default.Send<MessageOne>(msg);

            return null;
        }
    }
}
