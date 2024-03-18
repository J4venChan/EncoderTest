using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncoderTest
{
    public class SensorCommand
    {
        public static readonly byte[] cmdSetFrequency500Hz = [0x00, 0x06, 0x04, 0x00, 0x00, 0x00, 0x89, 0x2B];  //设置500KHz指令;
        public static readonly byte[] cmdSetFrequency250Hz = [0x00, 0x06, 0x04, 0x00, 0x00, 0x00, 0xC9, 0x6B];  //设置250KHz指令;
        public static readonly byte[] cmdSetDeviceAddress = [0x00, 0x06, 0x01, 0x00, 0x00, 0xFF, 0xC9, 0xA7];  //设置设备地址 FF;


        public static readonly byte[] cmdGetPosition = [0xFF, 0x03, 0x00, 0x01, 0x00, 0x02, 0x80, 0x15];  //获取位置值;
        public static readonly byte[] cmdGetDeviceAddress = [0x00, 0x03, 0x01, 0x00, 0x00, 0x01, 0x84, 0x27];  //获取设备地址;

        public static int DecodePositionFromReceiveData(byte[] receiveData)
        {
            int position = 0;
            byte[] dataBytes = [receiveData[4], receiveData[3], receiveData[6], receiveData[5]];//高低交换，前后交换
            position = BitConverter.ToInt32(dataBytes, 0);
            Console.WriteLine(position);
            return position;
        }

    }
}
