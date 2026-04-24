using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";
            string pstPath = "output.pst";

            // Verify input EML file exists
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{emlPath}' not found.");
                return;
            }

            // If a PST file already exists, attempt to delete it
            if (File.Exists(pstPath))
            {
                try
                {
                    File.Delete(pstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unable to delete existing PST file: {ex.Message}");
                    return;
                }
            }

            // Load the MailMessage from the EML file
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Convert MailMessage to MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Create a new PST file (Unicode format)
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        try
                        {
                            // Add the converted message to the root folder of the PST
                            string entryId = pst.RootFolder.AddMessage(mapiMessage);
                            Console.WriteLine($"Message added to PST. EntryId: {entryId}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
