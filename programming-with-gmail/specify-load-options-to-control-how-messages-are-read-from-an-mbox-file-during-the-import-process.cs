using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Path to the MBOX file to be processed
            string mboxFilePath = "sample.mbox";

            // Verify that the MBOX file exists before attempting to read it
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxFilePath}");
                return;
            }

            // Configure load options for the MBOX storage
            MboxLoadOptions mboxLoadOptions = new MboxLoadOptions
            {
                // Keep the underlying stream closed after the reader is disposed
                LeaveOpen = false,
                // Use UTF-8 as the preferred encoding for message bodies
                PreferredTextEncoding = Encoding.UTF8
            };

            // Configure load options for individual EML messages extracted from the MBOX
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions
            {
                // Preserve TNEF attachments if present
                PreserveTnefAttachments = true,
                // Use UTF-8 for message subject and body
                PreferredTextEncoding = Encoding.UTF8
            };

            // Create the MBOX reader with the specified file and load options
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, mboxLoadOptions))
            {
                // Enumerate each message using the EML load options
                foreach (MailMessage message in mboxReader.EnumerateMessages(emlLoadOptions))
                {
                    // Ensure each MailMessage is disposed after use
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors and report them without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}