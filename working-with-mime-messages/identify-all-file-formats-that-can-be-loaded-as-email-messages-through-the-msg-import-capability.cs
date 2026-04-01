using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string[] supportedFormats = new string[] { "EML", "HTML", "MHTML", "MSG", "DAT" };
            Console.WriteLine("Supported file formats for MSG import capability:");
            foreach (string format in supportedFormats)
            {
                Console.WriteLine("- " + format);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
