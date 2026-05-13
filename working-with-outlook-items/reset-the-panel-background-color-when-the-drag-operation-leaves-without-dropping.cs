using System;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Simulate the start of a drag operation.
                Console.WriteLine("Drag operation started.");

                // Simulate the drag leaving the panel without a drop.
                OnDragLeave();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        // This method represents the handler that would be invoked when the drag
        // operation leaves the panel without dropping. In a UI application it would
        // reset the panel's background color; here we simply log the action.
        static void OnDragLeave()
        {
            Console.WriteLine("Drag left the panel without dropping. Resetting background color.");
        }
    }
}
