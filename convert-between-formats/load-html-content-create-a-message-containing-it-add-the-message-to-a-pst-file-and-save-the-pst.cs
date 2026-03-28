using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for the HTML source and the PST file
            string htmlPath = "message.html";
            string pstPath = "sample.pst";

            // Ensure the HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Placeholder</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Read the HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Open existing PST or create a new one if it does not exist
            PersonalStorage pst;
            if (File.Exists(pstPath))
            {
                try
                {
                    pst = PersonalStorage.FromFile(pstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to open PST file: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Ensure the PST object is disposed properly
            using (pst)
            {
                // Create a new MAPI message and set its HTML body
                using (MapiMessage message = new MapiMessage())
                {
                    message.Subject = "HTML Message";
                    message.SetBodyContent(htmlContent, BodyContentType.Html);

                    // Add the message to the root folder of the PST
                    string entryId = pst.RootFolder.AddMessage(message);
                    Console.WriteLine($"Message added with EntryId: {entryId}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
