using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = @"C:\Emails\sample.msg";

            // Verify that the MSG file exists before attempting to load it.
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file and extract its plain‑text body.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                string plainTextBody = message.Body ?? string.Empty;
                Console.WriteLine("Plain‑text Body:");
                Console.WriteLine(plainTextBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
