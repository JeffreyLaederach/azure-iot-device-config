using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Services.Peripherals.LCD
{
    internal interface ILCD
    {
        public void Clear();

        // Method to write data to a specific location on the display
        // Parameters:
        // col - the column index where the data should be written
        // row - the row index where the data should be written
        // data - the string data to be written to the display
        
        public void Write(int col, int row, string data);

        public void Dispose();
       
    }
}
