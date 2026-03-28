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
            // Path to the MSG file
            string msgFilePath = "message.msg";

            // Verify that the input file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Open the file stream and load the MSG message
            using (FileStream fileStream = new FileStream(msgFilePath, FileMode.Open, FileAccess.Read))
            {
                using (MapiMessage message = MapiMessage.Load(fileStream))
                {
                    // Output basic properties of the message
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("Sender Email: " + message.SenderEmailAddress);
                    Console.WriteLine("Body: " + message.Body);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
