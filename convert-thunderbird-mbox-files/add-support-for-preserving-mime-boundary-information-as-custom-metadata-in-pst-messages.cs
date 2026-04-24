using System;
using System.IO;
using System.Text;
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
            string emlPath = "sample.eml";
            string pstPath = "archive.pst";

            // Ensure the EML file exists; create a minimal placeholder if missing
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

                try
                {
                    string placeholder = "Subject: Placeholder\r\nFrom: placeholder@example.com\r\nTo: placeholder@example.com\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholder, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Extract MIME boundary from the Content-Type header, if present
            string mimeBoundary = string.Empty;
            string contentTypeHeader = mailMessage.Headers["Content-Type"];
            if (!string.IsNullOrEmpty(contentTypeHeader))
            {
                int boundaryIndex = contentTypeHeader.IndexOf("boundary=", StringComparison.OrdinalIgnoreCase);
                if (boundaryIndex >= 0)
                {
                    int start = boundaryIndex + "boundary=".Length;
                    // Trim possible surrounding quotes
                    if (start < contentTypeHeader.Length && (contentTypeHeader[start] == '"' || contentTypeHeader[start] == '\''))
                    {
                        start++;
                        int endQuote = contentTypeHeader.IndexOf(contentTypeHeader[start - 1], start);
                        if (endQuote > start)
                        {
                            mimeBoundary = contentTypeHeader.Substring(start, endQuote - start);
                        }
                    }
                    else
                    {
                        int end = contentTypeHeader.IndexOf(';', start);
                        if (end == -1) end = contentTypeHeader.Length;
                        mimeBoundary = contentTypeHeader.Substring(start, end - start).Trim();
                    }
                }
            }

            // Convert MailMessage to MapiMessage
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = MapiMessage.FromMailMessage(mailMessage);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert MailMessage to MapiMessage: {ex.Message}");
                return;
            }

            // Add custom property with MIME boundary information (if any)
            if (!string.IsNullOrEmpty(mimeBoundary))
            {
                try
                {
                    byte[] boundaryBytes = Encoding.Unicode.GetBytes(mimeBoundary);
                    mapiMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, boundaryBytes, "MimeBoundary");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add custom MIME boundary property: {ex.Message}");
                    // Continue without failing the whole operation
                }
            }

            // Ensure PST file exists; create a new one if missing
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

            // Open PST and add the message to the Inbox folder
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    string entryId = inboxFolder.AddMessage(mapiMessage);
                    Console.WriteLine($"Message added to PST with EntryId: {entryId}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
