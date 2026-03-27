using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Output basic properties of the loaded message
                Console.WriteLine("Subject: " + message.Subject);
                Console.WriteLine("From: " + message.SenderName);
                Console.WriteLine("Body: " + message.Body);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
