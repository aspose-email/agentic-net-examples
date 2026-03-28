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
            // Define file paths
            string emlPath = "message.eml";
            string pstPath = "output.pst";

            // Ensure the EML file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    string placeholder = "From: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Ensure the PST file exists; create a new one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Load the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Load the EML message
                using (MailMessage mailMessage = MailMessage.Load(emlPath))
                {
                    // Convert MailMessage to MapiMessage
                    MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                    // Add the message to the root folder of the PST
                    try
                    {
                        string entryId = pst.RootFolder.AddMessage(mapiMessage);
                        Console.WriteLine($"Message added to PST with EntryId: {entryId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                        return;
                    }
                }

                // Save changes to the PST file
                try
                {
                    pst.SaveAs(pstPath, FileFormat.Pst);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save PST file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
