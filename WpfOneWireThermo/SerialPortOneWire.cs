using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public Boolean Init()
        {
            try
            {
                this.serialPort.DtrEnable = true;
                this.serialPort.Open();
            } catch(Exception exception)
            {
                Console.Error.WriteLine(exception);
                
            }
            return serialPort.IsOpen;
        }
        public Boolean Deinit()
        {
            try
            {
                serialPort.Close();
            } catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            
            }
            return serialPort.IsOpen;
        }
        public void SetConfiguration()
        {

        }
    }
}
