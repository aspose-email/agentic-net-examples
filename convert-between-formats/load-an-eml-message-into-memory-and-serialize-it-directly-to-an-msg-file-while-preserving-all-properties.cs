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
            string emlPath = "input.eml";
            string msgPath = "output.msg";

            // Verify that the input EML file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            try
            {
                // Load the EML message into a MailMessage object
                using (MailMessage mailMessage = MailMessage.Load(emlPath))
                {
                    // Convert the MailMessage to a MapiMessage preserving all properties
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Save the MapiMessage as an MSG file
                        mapiMessage.Save(msgPath);
                        Console.WriteLine($"Successfully converted '{emlPath}' to '{msgPath}'.");
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"IO error: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
