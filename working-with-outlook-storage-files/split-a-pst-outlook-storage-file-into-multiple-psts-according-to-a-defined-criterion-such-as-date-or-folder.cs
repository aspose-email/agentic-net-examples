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
            string inputPstPath = "input.pst";
            string outputFolder = "output";

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Guard input PST file; create minimal placeholder if missing
            if (!File.Exists(inputPstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                    {
                        // Add a simple message to the root folder
                        MapiMessage msg = new MapiMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder message.");
                        placeholder.RootFolder.AddMessage(msg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
            {
                // Split the PST into parts of approximately 10 MB each
                long chunkSize = 10L * 1024 * 1024; // 10 MB
                try
                {
                    pst.SplitInto(chunkSize, outputFolder);
                    Console.WriteLine("PST split operation completed successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during PST split: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
