using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using System.Device.Gpio;
using System.Device.I2c;

namespace IoT.Services.Peripherals.LCD
{
    internal class LCD:ILCD
    {
        // Private fields to hold the GPIO expander and the LCD instance
        private Pcf8574 _gpioExpander;
        private Lcd1602 _lcd;

        public LCD() 
        {
            // Create an I2C connection to the GPIO expander
            I2cDevice gpioExpanderI2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x3f));
           
            // Initialize the GPIO expander
            _gpioExpander = new Pcf8574(gpioExpanderI2cDevice);

            // Initialize the LCD instance with specific configuration
            _lcd = new Lcd1602(registerSelectPin: 0,
                                    enablePin: 2,
                                    dataPins: new int[] { 4, 5, 6, 7 },
                                    backlightPin: 3,
                                    backlightBrightness: 0.1f,
                                    readWritePin: 1,
                                    controller: new GpioController(PinNumberingScheme.Logical, _gpioExpander));
        }
 
        public void Dispose()
        {
            _lcd.Dispose(); 
            _gpioExpander.Dispose(); 
        }

        public void Clear()
        {
            _lcd.Clear();  
        }

        // Implementation of the Write method to write data to the LCD at a specific position
        // Parameters:
        //   col - the column index where the data should be written
        //   row - the row index where the data should be written
        //   data - the string data to be written to the display
        
        public void Write(int col, int row, string data)
        {
            _lcd.SetCursorPosition(col, row);
            _lcd.Write(data);
        }
    }
}
