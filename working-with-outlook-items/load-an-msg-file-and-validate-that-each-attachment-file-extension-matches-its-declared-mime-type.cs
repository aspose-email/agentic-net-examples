using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Ensure the file exists; create a minimal placeholder if it does not
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder",
                        "This is a placeholder MSG file."))
                    {
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Placeholder MSG created at: {msgPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Mapping of common MIME types to expected file extensions (without leading dot)
                Dictionary<string, string> mimeToExtension = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "image/jpeg", "jpg" },
                    { "image/png", "png" },
                    { "application/pdf", "pdf" },
                    { "text/plain", "txt" },
                    { "application/msword", "doc" },
                    { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx" },
                    { "application/vnd.ms-excel", "xls" },
                    { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx" }
                };

                // Iterate through each attachment and validate extension vs MIME type
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string fileExtension = attachment.Extension?.TrimStart('.'); // e.g., "pdf"
                    string mimeTag = attachment.MimeTag; // e.g., "application/pdf"

                    if (string.IsNullOrEmpty(fileExtension) || string.IsNullOrEmpty(mimeTag))
                    {
                        Console.WriteLine($"Attachment \"{attachment.FileName}\" lacks extension or MIME information.");
                        continue;
                    }

                    if (mimeToExtension.TryGetValue(mimeTag, out string expectedExtension))
                    {
                        if (!string.Equals(fileExtension, expectedExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Mismatch detected: Attachment \"{attachment.FileName}\" has extension \"{fileExtension}\" but MIME type \"{mimeTag}\" suggests \"{expectedExtension}\".");
                        }
                        else
                        {
                            Console.WriteLine($"Attachment \"{attachment.FileName}\" passes validation (extension matches MIME type).");
                        }
                    }
                    else
                    {
                        // If MIME type is unknown, just report the values
                        Console.WriteLine($"Attachment \"{attachment.FileName}\": extension \"{fileExtension}\", MIME \"{mimeTag}\" (no reference mapping available).");
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
