using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FD_FE;

namespace FD_Tools.Connection
{
    public class Connection
    {
        public bool is_host = true;
        private IPAddress ip = IPAddress.Any;
        public IPAddress GetIP() => ip;
        public bool SetIP(IPAddress IP) { ip = (is_host) ? IPAddress.Any : IP; return true; }
        public bool SetIP(string IP)
        {
            if (is_host) { ip = IPAddress.Any; return true; }
            else return IPAddress.TryParse(IP, out ip);
        }

        public TcpListener server = null;
        public TcpClient client = null;
        public Socket socket = null;

        public delegate void InterruptionEventHandler(string message);
        public event InterruptionEventHandler ConnectionInterrupted;
        public event InterruptionEventHandler SendRecieveInterrupted;
        public async Task<byte[]> ReceiveData(int n, Socket skt = null) => await Task<byte[]>.Run(() =>
        {
            if (skt == null) skt = socket;
            try
            {
                byte[] data = new byte[n];
                socket.Receive(data);
                return data;
            }
            catch (Exception ex)
            {
                SendRecieveInterrupted?.Invoke(ex.Message);
                return null;
            }
        });
        public async Task<short[]> ReceiveDataS(int n, Socket skt = null) => await Task<short[]>.Run(() =>
        {
            if (skt == null) skt = socket;
            try
            {
                byte[] data = new byte[2 * n];
                short[] res = new short[n];
                socket.Receive(data);
                for (int i = 0; i < n; i++)
                {
                    res[i] = (short)(((data[i * 2 + 1]) << 8) + data[i * 2]);
                }
                return res;
            }
            catch (Exception ex)
            {
                SendRecieveInterrupted?.Invoke(ex.Message);
                return null;
            }
        });
        //public async Task<object[]> ReceiveData(int n, int size) => await Task<object[]>.Run(()=>
        //{
        //    try
        //    {
        //        byte[] data = new byte[size*n];
        //        object[] res = new object[n];
        //        socket.Receive(data);
        //        long e = 0;
        //        for (int i=0; i<n; i++)
        //        {
        //            for (int j=0;  j<size; j++)
        //            {
        //                e = e << 8 + data[i*size + j];
        //            }
        //            res[i] = e;
        //        }
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return null;
        //    }
        //});
        //public void SendData(object[] data, int n, int size=2)
        //{
        //    byte[] res = new byte[size * n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        long e = (long)data[i];
        //        for (int j = 0; j < size; j++)
        //        {
        //            res[i*size + j] = (byte)e;
        //            e = e >> 8;
        //        }
        //    }
        //    try { socket.Send(res); }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        public void SendData(byte[] data, Socket skt = null)
        {
            if (skt == null) skt = socket;
            try { skt.Send(data); }
            catch (Exception ex)
            {
                SendRecieveInterrupted?.Invoke(ex.Message);
            }
        }
        public void SendData(short[] data, int n, Socket skt = null)
        {
            if (skt == null) skt = socket;
            byte[] res = new byte[2 * n];
            for (int i = 0; i < n; i++)
            {
                res[i * 2] = (byte)data[i];
                res[i * 2 + 1] = (byte)(data[i] >> 8);
            }
            try { skt.Send(res); }
            catch (Exception ex) {
                SendRecieveInterrupted?.Invoke(ex.Message);
            }
        }

        public async Task<bool> Connect()
        {
            if (is_host)
            {
                server = new TcpListener(ip, 4013);
                server.Start();
                return await Task<bool>.Run(() => {
                    try
                    {
                        socket = server.AcceptSocket();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        server?.Stop();
                        ConnectionInterrupted?.Invoke(ex.Message);
                        return false;
                    }
                });
            }
            else
            {
                return await Task<bool>.Run(() =>
                {
                    try
                    {
                        client = new TcpClient(ip.ToString(), 4013);
                        socket = client.Client;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        client?.Close();
                        ConnectionInterrupted?.Invoke(ex.Message);
                        return false;
                    }
                });
            }
        }
        public void Disconnect()
        {
            ip = IPAddress.Any;
            socket?.Close();
            server?.Stop();
            client?.Close();
        }
    }
}