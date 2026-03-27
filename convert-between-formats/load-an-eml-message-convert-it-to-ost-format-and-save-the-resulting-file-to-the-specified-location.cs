using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "input.eml";
            string ostPath = "output.ost";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Convert to MAPI message
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                // Create an OST (or PST) storage file
                using (PersonalStorage storage = PersonalStorage.Create(ostPath, FileFormatVersion.Unicode))
                {
                    // Add the message to the root folder
                    storage.RootFolder.AddMessage(mapiMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
