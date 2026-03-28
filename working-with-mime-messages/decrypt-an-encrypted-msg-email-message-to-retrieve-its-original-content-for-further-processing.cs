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
            string msgPath = @"c:\temp\encrypted.msg";

            // Verify that the input file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage originalMessage = MapiMessage.Load(msgPath))
            {
                // Check if the message is encrypted
                if (!originalMessage.IsEncrypted)
                {
                    Console.WriteLine("The message is not encrypted.");
                    Console.WriteLine($"Subject: {originalMessage.Subject}");
                    Console.WriteLine($"Body: {originalMessage.Body}");
                    return;
                }

                // Decrypt the message (searches user/computer stores for the certificate)
                using (MapiMessage decryptedMessage = originalMessage.Decrypt())
                {
                    Console.WriteLine("Message decrypted successfully.");
                    Console.WriteLine($"Subject: {decryptedMessage.Subject}");
                    Console.WriteLine($"Body: {decryptedMessage.Body}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
