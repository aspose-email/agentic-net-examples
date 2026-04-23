using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define paths and custom prefix
                string pstPath = "sample.pst";
                string outputDirectory = "OutputFiles";
                string customPrefix = "Custom_";

                // Ensure output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Create a new PST file (Unicode format)
                try
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a predefined folder inside the PST
                        FolderInfo customFolder = pst.CreatePredefinedFolder("CustomFolder", StandardIpmFolder.Inbox);

                        // Create a simple MAPI message
                        MapiMessage message = new MapiMessage(
                            "sender@example.com",
                            "recipient@example.com",
                            "Test Subject",
                            "This is the body of the test message."
                        );

                        // Add the message to the custom folder
                        string entryId = customFolder.AddMessage(message);

                        // Extract the message back from the PST
                        using (MapiMessage extractedMessage = pst.ExtractMessage(entryId))
                        {
                            // Build the file name with the custom prefix
                            string fileName = customPrefix + extractedMessage.Subject + ".msg";
                            string filePath = Path.Combine(outputDirectory, fileName);

                            // Save the extracted message to the file system
                            try
                            {
                                extractedMessage.Save(filePath);
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message file: {saveEx.Message}");
                                return;
                            }
                        }
                    }
                }
                catch (Exception pstEx)
                {
                    Console.Error.WriteLine($"PST operation failed: {pstEx.Message}");
                    return;
                }

                // Unit test: verify that each file in the output directory starts with the custom prefix
                try
                {
                    string[] files = Directory.GetFiles(outputDirectory);
                    bool allMatch = true;
                    foreach (string file in files)
                    {
                        string fileNameOnly = Path.GetFileName(file);
                        if (!fileNameOnly.StartsWith(customPrefix, StringComparison.Ordinal))
                        {
                            Console.Error.WriteLine($"File '{fileNameOnly}' does not start with the required prefix '{customPrefix}'.");
                            allMatch = false;
                        }
                    }

                    if (allMatch)
                    {
                        Console.WriteLine("Unit test passed: all file names start with the custom prefix.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Unit test failed: some file names do not have the required prefix.");
                    }
                }
                catch (Exception testEx)
                {
                    Console.Error.WriteLine($"Unit test encountered an error: {testEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
