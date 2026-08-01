using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Storage.Mbox;

namespace ThunderbirdIntegrationDemo
{
    class Program
    {
        static void Main()
        {
            // Define the path to a Thunderbird mbox file (Inbox example).
            string mboxPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Thunderbird",
                "Profiles",
                "default-release",
                "Mail",
                "Local Folders",
                "Inbox",
                "Inbox.mbox");

            // Verify that the mbox file exists before proceeding.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Mbox file not found: {mboxPath}");
                return;
            }

            try
            {
                // Open the Thunderbird mbox (mboxrd format) for reading.
                using (MboxrdStorageReader reader = new MboxrdStorageReader(mboxPath, new MboxLoadOptions()))
                {
                    Console.WriteLine("Messages in the Thunderbird mbox:");
                    foreach (MailMessage message in reader.EnumerateMessages())
                    {
                        Console.WriteLine($"- Subject: {message.Subject}");
                        Console.WriteLine($"  From   : {message.From}");
                        Console.WriteLine($"  To     : {string.Join(", ", message.To)}");
                    }
                }

                // Compose a new email message.
                MailMessage newMessage = new MailMessage
                {
                    From = new MailAddress("sender@example.com"),
                    Subject = "Demo message from Aspose.Email",
                    Body = "This message was created and saved to a Thunderbird mbox file using Aspose.Email."
                };
                newMessage.To.Add(new MailAddress("recipient@example.com"));

                // Define the output mbox file path.
                string outputMboxPath = Path.Combine(Environment.CurrentDirectory, "output.mbox");

                // Ensure the directory for the output file exists.
                string outputDir = Path.GetDirectoryName(outputMboxPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Write the new message to the output mbox (mboxrd format).
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(outputMboxPath, new MboxSaveOptions()))
                {
                    writer.WriteMessage(newMessage);
                }

                Console.WriteLine($"New message saved to: {outputMboxPath}");
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors and report them without crashing.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
