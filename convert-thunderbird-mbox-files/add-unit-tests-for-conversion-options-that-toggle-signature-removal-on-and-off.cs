using Aspose.Email;
using System;
using Aspose.Email.Storage;
using Aspose.Email.Mapi;

namespace AsposeEmailSignatureToggleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestMboxToPstConversionOptions();
                TestMapiConversionOptions();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }

        private static void TestMboxToPstConversionOptions()
        {
            // Create options with signature removal disabled
            MboxToPstConversionOptions optionsWithoutRemoval = new MboxToPstConversionOptions();
            optionsWithoutRemoval.RemoveSignature = false;
            Console.WriteLine($"MboxToPstConversionOptions.RemoveSignature (expected false): {optionsWithoutRemoval.RemoveSignature}");

            // Create options with signature removal enabled
            MboxToPstConversionOptions optionsWithRemoval = new MboxToPstConversionOptions();
            optionsWithRemoval.RemoveSignature = true;
            Console.WriteLine($"MboxToPstConversionOptions.RemoveSignature (expected true): {optionsWithRemoval.RemoveSignature}");
        }

        private static void TestMapiConversionOptions()
        {
            // Create options with signature removal disabled
            MapiConversionOptions mapiOptionsWithoutRemoval = new MapiConversionOptions();
            mapiOptionsWithoutRemoval.RemoveSignature = false;
            Console.WriteLine($"MapiConversionOptions.RemoveSignature (expected false): {mapiOptionsWithoutRemoval.RemoveSignature}");

            // Create options with signature removal enabled
            MapiConversionOptions mapiOptionsWithRemoval = new MapiConversionOptions();
            mapiOptionsWithRemoval.RemoveSignature = true;
            Console.WriteLine($"MapiConversionOptions.RemoveSignature (expected true): {mapiOptionsWithRemoval.RemoveSignature}");
        }
    }
}
