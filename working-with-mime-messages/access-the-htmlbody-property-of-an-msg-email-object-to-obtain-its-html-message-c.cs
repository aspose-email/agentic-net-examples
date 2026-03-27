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
            string msgPath = "message.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file and access its HTML body
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                string htmlBody = msg.BodyHtml;
                Console.WriteLine("HTML Body:");
                Console.WriteLine(htmlBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
