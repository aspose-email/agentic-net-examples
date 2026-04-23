using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Guard input MBOX file existence
            if (!File.Exists(mboxPath))
            {
                // Create an empty placeholder MBOX file
                try
                {
                    using (FileStream fs = File.Create(mboxPath))
                    {
                        // No content needed
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Guard output PST directory existence
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Get the original MBOX file creation date
            DateTime mboxCreationDate = File.GetCreationTime(mboxPath);
            string creationDateString = mboxCreationDate.ToString("o"); // ISO 8601 format

            // Define a handler to tag each message with the creation date
            MailStorageConverter.MailHandler handler = (MailMessage message) =>
            {
                // Add a custom header to preserve the original MBOX creation date
                message.Headers.Add("X-Original-Mbox-Creation-Date", creationDateString);
            };

            // Perform the conversion with the handler
            try
            {
                MailStorageConverter.MboxToPst(mboxPath, pstPath, handler);
                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
