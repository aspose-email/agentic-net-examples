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
            // Define paths
            string ostPath = "sample.ost";
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(ostPath));
            string htmlContent = "<html><body><h1>Hello from Aspose.Email</h1></body></html>";

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {ex.Message}");
                    return;
                }
            }

            // Ensure OST file exists; create a new one if missing
            if (!File.Exists(ostPath))
            {
                try
                {
                    // Create a new Unicode OST/PST file
                    PersonalStorage.Create(ostPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create OST file '{ostPath}': {ex.Message}");
                    return;
                }
            }

            // Open the OST file
            using (PersonalStorage pst = PersonalStorage.FromFile(ostPath))
            {
                // Create a new MAPI message from raw HTML
                MapiMessage mapiMessage = new MapiMessage();
                mapiMessage.Subject = "Sample HTML Message";
                // Set HTML body content
                mapiMessage.SetBodyContent(htmlContent, BodyContentType.Html);
                // Optionally set sender information
                mapiMessage.SenderName = "Aspose Sample";
                mapiMessage.SenderEmailAddress = "sample@aspose.com";

                // Add the message to the root folder
                try
                {
                    string entryId = pst.RootFolder.AddMessage(mapiMessage);
                    Console.WriteLine($"Message added with EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add message to OST: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine("OST file updated successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
