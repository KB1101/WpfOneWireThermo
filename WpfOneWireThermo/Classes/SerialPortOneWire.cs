using System;
using System.IO.Ports;

namespace WpfOneWireThermo
{
    class SerialPortOneWire
    {
        private SerialPort serialPort; // serial port handle

        // serial port configuration for OneWire communication via RS232 
        private String portName = "COM1";
        private const Int32 baudRate = 115200;
        private const Parity parity = Parity.None;
        private const Int32 dataBits = 8;
        private const StopBits stopBits = StopBits.One;

        private String[] portNames; 

        public SerialPortOneWire()
        {
            serialPort = new SerialPort();
            serialPort.ReadTimeout = serialPort.WriteTimeout = 1000;
        }
        public SerialPortOneWire(String portName)
        {
            serialPort = new SerialPort(portName,baudRate,parity,dataBits,stopBits);
            serialPort.ReadTimeout = serialPort.WriteTimeout = 1000;
        }
        ~SerialPortOneWire()
        {
            if (this.serialPort.IsOpen) this.serialPort.Close();
            this.serialPort.Dispose();
            this.serialPort = null;
        }
        /*
         * The port names are obtained from the system registry. 
         * If the registry contains stale or otherwise incorrect data then AviavailablePorts method will return incorrect data.
         * The order of port names returned from GetPortNames is not specified.
         * This method returns String[]
         */
        public String[] AviavailablePorts()
        {
            portNames = SerialPort.GetPortNames();
            return portNames;
        }
        public Boolean OneWireInit()
        {
            try
            {
                this.serialPort.DtrEnable = true;
                this.serialPort.Open();
            } catch(Exception exception)
            {
                Console.Error.WriteLine(exception);
                throw exception;
                
            }
            return serialPort.IsOpen;
        }
        public Boolean OneWireDeinit()
        {
            try
            {
                serialPort.Close();
            } catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
                throw exception;
            
            }
            return serialPort.IsOpen;
        }
        public void SetPortName(String portName)
        {
            try
            {
                Boolean portState = serialPort.IsOpen;
                if (portState) serialPort.Close();
                serialPort.PortName = portName;
                this.portName = portName;
                if (portState) serialPort.Open();
            } catch(Exception ex)
            {
                throw ex;
            }
        }
        public void WriteByte(byte bajt)
        {
            byte[] temp = new byte[8];
            for(var i = 0; i < 8; i++)
            {
                temp[i] = ( (int)(bajt & 0x01) == 1) ? (byte)0xFF : (byte)0x00;
                bajt = (byte)((int)bajt >> 1);
            }
            try
            {
                if (!serialPort.IsOpen) serialPort.Open();
                serialPort.DtrEnable = true;
                serialPort.Write(temp, 0, 8);
            } catch(TimeoutException tex)
            {
                Console.Error.WriteLineAsync(tex.ToString());
                throw tex;
            } 
         }
        public byte ReadByte()
        {
            byte[] temp = new byte[8];
            int bajt = 0;
            try
            {
                if (!serialPort.IsOpen) serialPort.Open();
                serialPort.DtrEnable = true;
                serialPort.Read(temp, 0, 8);

                for (var i = 0; i < 8; i++)
                {
                    bajt = bajt << 1;
                    bajt += (temp[7 - i] == (byte)0xFF) ? 1 : 0;
                }
            } catch (TimeoutException tex)
            {
                Console.Error.WriteLineAsync(tex.ToString());
                throw tex;
            }
            return (byte)bajt;
        }
        public byte Reset(Boolean rfb = true)
        {
            byte returnedByte = 0xFF;
            byte[] bajt = new byte[1];
            bajt[0] = 0xF0;

            try
            {
                serialPort.BaudRate = 9600;
                if (!serialPort.IsOpen) serialPort.Open();
                serialPort.DtrEnable = true;

                serialPort.Write(bajt, 0, 1);
                if(rfb) returnedByte = this.ReadByte();
                else returnedByte = (byte)serialPort.ReadByte();

                serialPort.BaudRate = 115200;

            } catch (TimeoutException tex)
            {
                Console.Error.WriteLineAsync(tex.ToString());
                throw tex;
            }
            return returnedByte;
        }
    }
}
