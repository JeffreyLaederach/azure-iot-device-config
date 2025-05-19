namespace IoT.Services.Peripherals.LCD

{
    internal class MockLCD:ILCD
    {
        public void Dispose()
        {
            Console.WriteLine("Disposing LCD");
        }

        public void Clear()
        {
            Console.WriteLine("Clearing LCD");
        }

        // Parameters:
        //   col: The column position on the LCD
        //   row: The row position on the LCD
        //   data: The data string to write to the LCD

        public void Write(int col, int row, string data)
        {
            Console.WriteLine($"{col}{row}: {data}");
        }
    }
}
