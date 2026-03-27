using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxFilePath = "output.mbox";

            // Ensure the directory for the MBOX file exists
            string directoryPath = Path.GetDirectoryName(mboxFilePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {ex.Message}");
                    return;
                }
            }

            // Prepare MBOX writer options
            MboxSaveOptions saveOptions = new MboxSaveOptions();

            // Write messages to the MBOX file
            try
            {
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxFilePath, saveOptions))
                {
                    // First message
                    MailMessage message1 = new MailMessage();
                    message1.From = new MailAddress("alice@example.com");
                    message1.To.Add(new MailAddress("bob@example.com"));
                    message1.Subject = "Hello Bob";
                    message1.Body = "Hi Bob,\nThis is a test email.\nRegards,\nAlice";

                    writer.WriteMessage(message1);
                    message1.Dispose();

                    // Second message
                    MailMessage message2 = new MailMessage();
                    message2.From = new MailAddress("carol@example.com");
                    message2.To.Add(new MailAddress("dave@example.com"));
                    message2.Subject = "Meeting Reminder";
                    message2.Body = "Dear Dave,\nDon't forget our meeting tomorrow at 10 AM.\nBest,\nCarol";

                    writer.WriteMessage(message2);
                    message2.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to write MBOX file – {ex.Message}");
                return;
            }

            Console.WriteLine($"MBOX file created successfully at: {mboxFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
