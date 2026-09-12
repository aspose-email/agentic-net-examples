using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

namespace MboxExample
{
    class Program
    {
        static void Main()
        {
            // Author note: This example demonstrates creating MailMessage objects,
            // writing them to an MBOX file using MboxrdStorageWriter, and then
            // reading them back with MboxStorageReader.

            string mboxPath = "output.mbox";

            // Ensure the directory for the MBOX file exists.
            try
            {
                string? directory = Path.GetDirectoryName(mboxPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare directory: {ex.Message}");
                return;
            }

            // Write messages to the MBOX file.
            try
            {
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, new MboxSaveOptions()))
                {
                    // First message
                    MailMessage message1 = new MailMessage();
                    message1.From = new MailAddress("alice@example.com");
                    message1.To.Add(new MailAddress("bob@example.com"));
                    message1.Subject = "Hello Bob";
                    message1.Body = "This is a test email.";

                    writer.WriteMessage(message1);

                    // Second message
                    MailMessage message2 = new MailMessage();
                    message2.From = new MailAddress("carol@example.com");
                    message2.To.Add(new MailAddress("dave@example.com"));
                    message2.Subject = "Meeting Reminder";
                    message2.Body = "Don't forget our meeting tomorrow at 10 AM.";

                    writer.WriteMessage(message2);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing MBOX file: {ex.Message}");
                return;
            }

            // Read messages back from the MBOX file.
            try
            {
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    MailMessage? msg;
                    while ((msg = reader.ReadNextMessage()) != null)
                    {
                        Console.WriteLine($"Read message: Subject=\"{msg.Subject}\", From=\"{msg.From}\", To=\"{msg.To}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading MBOX file: {ex.Message}");
            }
        }
    }
}
