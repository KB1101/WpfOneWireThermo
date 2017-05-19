using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; 

namespace WpfOneWireThermo
{
    /* Only one 1-Wire sensor on bus !!!!!! */
    class DS18B20
    {
        private SerialPortOneWire oneWire;
        public String[] oneWireAdapterPorts { get; }
        public DS18B20()
        {
            this.oneWire = new SerialPortOneWire();
            this.oneWireAdapterPorts = oneWire.AviavailablePorts();
            this.oneWire = null;
        }
        public void OneWireRun(String port)
        {
            this.oneWire = new SerialPortOneWire(port);
            try
            {
                this.oneWire.OneWireInit();
            } catch (Exception) { }
        }
        public byte[] GetSensorAddress()
        {
            byte[] address = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            try
            {
                if (oneWire.Reset() != 0xFF)
                {
                    oneWire.WriteByte(0x33);
                    if (oneWire.ReadByte() != 0x33) return address;

                    for (var i = 0; i < 8; i++)
                    {
                        oneWire.WriteByte(0xFF);
                        address[7 - i] = oneWire.ReadByte();
                    }

                }  
            } catch (Exception){
                throw new NotImplementedException();
            }

            return address;
        }
        public double GetSensorTemperature()
        {
            double result = -127F;
            try
            {
                byte resetState = oneWire.Reset();
                // if device is present
                if ((resetState >= 0x10 && resetState <= 0x90) || true)
                {
                    oneWire.WriteByte(0xCC); // Skip ROM command
                    if (oneWire.ReadByte() != 0xCC) return result;
                    oneWire.WriteByte(0x44); // Convert temperature command
                    if (oneWire.ReadByte() != 0x44) return result;

                    Thread.Sleep(800);// Wait for DS18B20 
                    

                    // Get the Data
                    oneWire.Reset();
                    oneWire.WriteByte(0xCC); // Skip ROM command
                    if (oneWire.ReadByte() != 0xCC) return result;
                    oneWire.WriteByte(0xBE); // Tell sensor to take a reading
                    if (oneWire.ReadByte() != 0xBE) return result;

                    byte LSB = 0xFF, MSB = 0xFF;
                    //start reading 
                    oneWire.WriteByte(0xFF);
                    LSB = oneWire.ReadByte();
                    oneWire.WriteByte(0xFF);
                    MSB = oneWire.ReadByte();

                    int rawTemperature = (UInt16)((UInt16)LSB | (UInt16)(MSB << 8));

                    // temperature conversion from int32 into double
                    result = 1F;
                    if ((rawTemperature & 0x8000) > 0)
                    {
                        rawTemperature = (rawTemperature ^ 0xffff) + 1;
                        result = -1F;
                    }
                    result *= (double)(6F * rawTemperature + rawTemperature / 4F) / 100F;
                }
 
            } catch (Exception)
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
