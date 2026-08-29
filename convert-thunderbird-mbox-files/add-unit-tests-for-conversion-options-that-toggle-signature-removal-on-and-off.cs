using System;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Mapi;
using System.Diagnostics;

namespace AsposeEmailSignatureToggleTests
{
    // Author: Generated example for toggling signature removal options
    class Program
    {
        static void Main()
        {
            try
            {
                TestMboxToPstConversionOptions();
                TestMapiConversionOptions();
                Console.WriteLine("All signature toggle tests passed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        static void TestMboxToPstConversionOptions()
        {
            // Create options with default settings
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();

            // Verify default is false
            if (options.RemoveSignature != false)
                throw new InvalidOperationException("Default RemoveSignature should be false.");

            // Enable signature removal
            options.RemoveSignature = true;
            if (options.RemoveSignature != true)
                throw new InvalidOperationException("RemoveSignature should be true after setting.");

            // Disable again
            options.RemoveSignature = false;
            if (options.RemoveSignature != false)
                throw new InvalidOperationException("RemoveSignature should be false after resetting.");
        }

        static void TestMapiConversionOptions()
        {
            // Create options with default settings
            MapiConversionOptions options = new MapiConversionOptions();

            // Verify default is false
            if (options.RemoveSignature != false)
                throw new InvalidOperationException("Default RemoveSignature should be false.");

            // Enable signature removal
            options.RemoveSignature = true;
            if (options.RemoveSignature != true)
                throw new InvalidOperationException("RemoveSignature should be true after setting.");

            // Disable again
            options.RemoveSignature = false;
            if (options.RemoveSignature != false)
                throw new InvalidOperationException("RemoveSignature should be false after resetting.");
        }
    }
}
