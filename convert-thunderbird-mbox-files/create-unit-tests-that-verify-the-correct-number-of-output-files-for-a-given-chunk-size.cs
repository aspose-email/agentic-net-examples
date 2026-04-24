using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string pstPath = "test.pst";
            string outputFolder = "chunks";

            // Clean previous artifacts
            if (File.Exists(pstPath))
            {
                try { File.Delete(pstPath); } catch { /* ignore */ }
            }

            if (Directory.Exists(outputFolder))
            {
                try
                {
                    foreach (string file in Directory.GetFiles(outputFolder))
                        File.Delete(file);
                    Directory.Delete(outputFolder);
                }
                catch { /* ignore */ }
            }

            // Ensure output folder exists
            try
            {
                Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Number of test messages
            const int messageCount = 5;

            // Create a new PST file and add test messages
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                FolderInfo rootFolder = pst.RootFolder;
                for (int i = 0; i < messageCount; i++)
                {
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("sender@example.com");
                    message.To.Add(new MailAddress("recipient@example.com"));
                    message.Subject = $"Test Message {i + 1}";
                    message.Body = "This is a test email.";
                    // Add the message to the PST root folder
                    rootFolder.AddMessage(MapiMessage.FromMailMessage(message));
                }
            }

            // Re-open the PST and split it into chunks with a very small chunk size
            // Using chunkSize = 1 forces each message into a separate PST part
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                long chunkSize = 1L;
                pst.SplitInto(chunkSize, outputFolder);
            }

            // Verify the number of output PST files
            if (!Directory.Exists(outputFolder))
            {
                Console.Error.WriteLine("Output folder was not created.");
                return;
            }

            string[] outputFiles = Directory.GetFiles(outputFolder, "*.pst");
            int actualFileCount = outputFiles.Length;
            int expectedFileCount = messageCount; // One file per message due to tiny chunk size

            Console.WriteLine($"Expected output files: {expectedFileCount}");
            Console.WriteLine($"Actual output files:   {actualFileCount}");

            if (actualFileCount == expectedFileCount)
            {
                Console.WriteLine("Test passed: Correct number of output files generated.");
            }
            else
            {
                Console.WriteLine("Test failed: Unexpected number of output files.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
