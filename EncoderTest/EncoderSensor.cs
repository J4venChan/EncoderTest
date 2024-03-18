using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EncoderTest
{
    public class EncoderSensor
    {
        private string sensorName;
        private string ip;
        private int port;
        private Socket socket;
        
        public bool IsConnected { get; set; }

        public EncoderSensor(string _ip,int _port, string _sensorName) 
        {
            sensorName = _sensorName;
            ip = _ip;
            port = _port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = 1000; // 设置接收超时时间为1秒
            socket.SendTimeout = 1000; // 设置发送超时时间为1秒

        }

        public void LogInfo(string info)
        {
            Console.WriteLine(sensorName + ":" + info);
        }

        public void Connect()
        {
            try
            {
                socket.Connect(ip, port);
                LogInfo("连接成功！");
                IsConnected = true;
            }
            catch (Exception ex)
            {
                LogInfo("连接失败！ " + ex.ToString());
            }
        }

        public int GetSensorPosition()
        {
            int position = 0;
            byte[] cmd = SensorCommand.cmdGetPosition;
            try
            {
                socket.Send(cmd);
                LogInfo("获取位置信息指令发送成功！");
            }
            catch (Exception ex)
            {
                LogInfo("获取位置信息指令发送失败！ " + ex.ToString());
            }

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = socket.Receive(buffer);
                byte[] messageBytes = new byte[bytesRead];
                Array.Copy(buffer, messageBytes, bytesRead);

                position = SensorCommand.DecodePositionFromReceiveData(messageBytes);
                LogInfo("当前位置为：" + position.ToString());
            }
            catch (Exception ex)
            {
                LogInfo("未收到位置反馈或位置解析失败！ " + ex.ToString());
            }
            return position;
        }



        public void GetClockFrequency(Socket sensor)
        {
            byte[] cmd = [0x00, 0x03, 0x04, 0x00, 0x00, 0x01, 0x84, 0xEB];

            try
            {
                sensor.Send(cmd);
                Console.WriteLine("读时钟频率指令发送成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
            }

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = sensor.Receive(buffer);
                byte[] messageBytes = new byte[bytesRead];
                Array.Copy(buffer, messageBytes, bytesRead);

                string hexString = BitConverter.ToString(messageBytes).Replace("-", " ");
                Console.WriteLine("收到时钟频率：" + hexString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误：" + ex.Message);
            }
        }

        public bool SetClockFrequency(Socket sensor)
        {
            byte[] cmd = [0x00, 0x06, 0x04, 0x00, 0x00, 0x00, 0x89, 0x2B];//设置500Khz
            //byte[] cmd = [0x00, 0x06, 0x04, 0x00, 0x00, 0xFF, 0xC9, 0x6B];//设置500Khz

            bool result = false;
            try
            {
                sensor.Send(cmd);
                Console.WriteLine("设置时钟频率指令发送成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
            }

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = sensor.Receive(buffer);
                byte[] messageBytes = new byte[bytesRead];
                Array.Copy(buffer, messageBytes, bytesRead);
                string hexString = BitConverter.ToString(messageBytes).Replace("-", " ");
                Console.WriteLine("收到时钟频率：" + hexString);

                if (messageBytes[5] == 0x00)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {
                result |= false;
                Console.WriteLine("错误：" + ex.Message);
            }
            return result;
        }


    }
}
