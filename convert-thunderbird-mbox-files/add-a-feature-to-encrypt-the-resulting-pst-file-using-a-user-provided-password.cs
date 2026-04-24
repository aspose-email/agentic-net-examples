using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path and password
            string pstFilePath = "output\\encrypted.pst";
            string pstPassword = "SecretPassword123";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(pstFilePath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new Unicode PST file
            using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
            {
                // Set password protection on the PST
                pst.Store.ChangePassword(pstPassword);

                // Create a simple MAPI message
                MapiMessage message = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    "This is a sample message body."
                );

                // Add the message to the root folder of the PST
                pst.RootFolder.AddMessage(message);
            }

            Console.WriteLine("PST file created and encrypted successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
