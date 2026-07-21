using System;
using System.Collections.Generic;
using Aspose.Email;

// Author: Aspose.Email example - list formats loadable via MSG import capability
class Program
{
    static void Main()
    {
        try
        {
            // Formats that MailMessage.Load can handle (including MSG import)
            List<string> loadableFormats = new List<string>
            {
                "EML  – MailMessage.Load with EmlLoadOptions",
                "HTML – MailMessage.Load with HtmlLoadOptions",
                "MHTML – MailMessage.Load with MhtmlLoadOptions",
                "MSG  – MailMessage.Load with MsgLoadOptions",
                "DAT  – MailMessage.Load with DatLoadOptions",
                "TNEF – MailMessage.Load with TnefLoadOptions"
            };

            Console.WriteLine("Supported email message formats that can be loaded via Aspose.Email:");
            foreach (string format in loadableFormats)
            {
                Console.WriteLine("- " + format);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
